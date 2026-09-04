import http from 'k6/http';
import { check } from 'k6';

const firstNames = [
  'Ahmet', 'Mehmet', 'Mustafa', 'Ali', 'Hasan', 'Hüseyin', 'İbrahim', 'Yusuf',
  'Ömer', 'Murat', 'Emre', 'Burak', 'Can', 'Kerem', 'Kaan', 'Mert',
  'Serkan', 'Hakan', 'Onur', 'Tolga', 'Barış', 'Umut', 'Volkan', 'Erkan',
  'Sinan', 'Okan', 'Berk', 'Alper', 'Eren', 'Furkan', 'Yasin', 'Arda',
  'Batuhan', 'Emir', 'Bora', 'Cihan', 'Deniz', 'Koray', 'Levent', 'Mahir',
  'Tarık', 'Uğur', 'Yavuz', 'Recep', 'Harun', 'Kadir', 'Samet', 'Suat',
  'Metin', 'Orhan',

  'Ayşe', 'Fatma', 'Elif', 'Zeynep', 'Merve', 'Ece', 'Selin', 'Gamze',
  'Büşra', 'Tuğba', 'Gizem', 'İrem', 'Cansu', 'Derya', 'Seda', 'Aylin',
  'Pınar', 'Melis', 'Aslı', 'Şule', 'Nazlı', 'Yasemin', 'Ceren', 'Gökçe',
  'Hande', 'Beyza', 'Sevgi', 'Nilay', 'Leyla', 'Rabia', 'Damla', 'Sinem',
  'Zehra', 'Sıla', 'Nurgül', 'Gül', 'Şeyma', 'Hatice', 'Özlem', 'İlayda',
  'Neslihan', 'Duygu', 'Eylül', 'Belgin', 'İlayda', 'Esra', 'Nihan', 'Sibel',
  'Yasemin', 'Cemile'
];
const lastNames = [
  'Yılmaz', 'Kaya', 'Demir', 'Şahin', 'Çelik', 'Yıldız', 'Yıldırım',
  'Öztürk', 'Aydın', 'Arslan', 'Doğan', 'Kılıç', 'Özkan', 'Yalçın',
  'Kaplan', 'Aktaş', 'Polat', 'Güneş', 'Avcı', 'Bulut', 'Tekin', 'Şen',
  'Kurt', 'Taş', 'Vural', 'Karaca', 'Erdem', 'Toprak', 'Şimşek', 'Koç',
  'Eren', 'Aslan', 'Çetin', 'Özdemir', 'Karaman', 'Ergin', 'Korkmaz',
  'Şeker', 'Türkmen', 'Acar', 'Keskin', 'Bayram', 'Tunç', 'Demirtaş',
  'Güler', 'Çakır', 'Sönmez', 'Yavuz', 'Akın', 'Uçar', 'Karataş',
  'Kara', 'Korkut', 'Bozkurt', 'Duman', 'Ekinci', 'Taşkın', 'Özer',
  'Ateş', 'Başar', 'Güven', 'Karaaslan', 'Özsoy', 'Köksal', 'Akgün',
  'Karaoğlu', 'Soylu', 'Gündüz', 'Çalışkan', 'Balcı', 'Korkut', 'Dağ',
  'Altun', 'Çoban', 'Genç', 'Özkan', 'Erdogan', 'Yücel', 'Koçak'
];



function getRandomItem() {
  const firstName = firstNames[Math.floor(Math.random() * firstNames.length)];
  const lastName = lastNames[Math.floor(Math.random() * lastNames.length)];

  return `${firstName} ${lastName}`;
}

function getRandomKey() {
  return Math.floor(10000 + Math.random() * 90000).toString(); // 5 haneli rastgele Key
}

function generateFakeEmail(name) {
  const cleanName = name.toLowerCase()
    .replace(/ğ/g, 'g').replace(/ü/g, 'u').replace(/ş/g, 's')
    .replace(/ı/g, 'i').replace(/ö/g, 'o').replace(/ç/g, 'c')
    .replace(/[^a-z]/g, '');
  const randomNum = Math.floor(Math.random() * 1000);
  return `${cleanName}${randomNum}@gmail.com`;
}

export const options = {
  scenarios: {
    send_users: {
      executor: 'per-vu-iterations',
      vus: 1,
      iterations: 800,
    },
  },
};

export default function () {
  const randomName = getRandomItem();

  const jsonBody = {
    Key: getRandomKey(),
    Name: randomName,
    Email: generateFakeEmail(randomName),
    Topics: [
      "notification:system"
    ]
  };

  const url = 'http://193.111.78.161:1020/integrationapi/users/create';
  const payload = JSON.stringify(jsonBody);
  const params = {
    headers: {
      'Content-Type': 'application/json',
      'Fluxify-Api-Key': 'xyz'
    },
  };

  const response = http.post(url, payload, params);

  check(response, {
    'status is 200 or 201': (r) => r.status === 200 || r.status === 201,
  });
}