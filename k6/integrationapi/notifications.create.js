import http from 'k6/http';
import { check } from 'k6';

// Lookup Değerleri
const types = ['error', 'warning', 'info', 'chat', 'fatal'];

// Title ve Summary için dinamik fake veri havuzu
const sampleData = {
  chat: [
    { title: "Bir mesajınız var", summary: "Mesut adlı kişiden bir mesajınız var" },
    { title: "Yeni sohbet isteği", summary: "Ayşe sizinle iletişime geçmek istiyor" },
    { title: "Grup Bildirimi", summary: "Yazılım ekibi grubunda yeni bir paylaşım yapıldı" },
  ],
  info: [
    { title: "Sistem Güncellemesi", summary: "Sistem bakımı başarıyla tamamlandı" },
    { title: "Profil Güncellendi", summary: "Hesap bilgileriniz başarıyla kaydedildi" },
    { title: "Hoş Geldiniz", summary: "Uygulamamıza ilk adımı attığınız için teşekkürler" },
  ],
  warning: [
    { title: "Şifre Süresi", summary: "Parolanızın süresi 3 gün içinde dolacak" },
    { title: "Yüksek Bellek Kullanımı", summary: "Sunucu bellek kullanımı %85 seviyesine ulaştı" },
    { title: "Oturum Açma Denemesi", summary: "Farklı bir cihazdan giriş yapıldı" },
  ],
  error: [
    { title: "İşlem Başarısız", summary: "Ödeme işlemi gerçekleştirilemedi" },
    { title: "Bağlantı Hatası", summary: "Veritabanı sunucusuna erişilemiyor" },
    { title: "Veri Yüklenemedi", summary: "İstenen kaynak bulunamadı (404)" },
  ],
  fatal: [
    { title: "Kritik Sistem Hatası", summary: "Çekirdek servis yanıt vermiyor, sistem durduruldu" },
    { title: "Veritabanı Çökmesi", summary: "Ana veritabanı bağlantısı tamamen kesildi" },
    { title: "Güvenlik İhlali Tespiti", summary: "Yetkisiz erişim nedeniyle servis kilitlendi" },
  ]
};

// Yardımcı Fonksiyon: Diziden rastgele eleman seçer
function getRandomItem(array) {
  return array[Math.floor(Math.random() * array.length)];
}

export const options = {
  scenarios: {
    send_notifications: {
      executor: 'per-vu-iterations',
      vus: 1,           // İhtiyaca göre sanal kullanıcı sayısını artırabilirsiniz
      iterations: 100,  // Toplam 100 istek gönderilir
    },
  },
};

export default function () {
  // 1. Lookup alanından rastgele 'Type' seçimi
  const selectedType = getRandomItem(types);

  // 2. Seçilen türe uygun rastgele Title ve Summary seçimi
  const content = getRandomItem(sampleData[selectedType]);

  // 3. İstenen JSON gövdesinin oluşturulması
  const jsonBody = {
    Title: content.title,
    Summary: content.summary,
    Type: selectedType,
    Url: null,
    Thumbnail: null,
    To: {
      Users: null,
      Topics: [
        "notification:system"
      ]
    }
  };

  const url = 'http://193.111.78.161:1020/integrationapi/notifications/create'; // Hedef endpoint adresiniz
  const payload = JSON.stringify(jsonBody);
  const params = {
    headers: {
      'Content-Type': 'application/json',
      'Fluxify-Api-Key': 'xyz'
    },
  };

  const response = http.post(url, payload, params);

  // İstek kontrolü
  check(response, {
    'status 200/201': (r) => r.status === 200 || r.status === 201,
  });
}