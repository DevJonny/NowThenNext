// NowThenNext Service Worker - Production
// Caches app assets for offline use and auto-updates

const CACHE_NAME = 'nowthenext-cache-v1';
const OFFLINE_URL = './';

// Assets to cache immediately on install
const PRECACHE_ASSETS = [
  './',
  './index.html',
  './manifest.webmanifest',
  './icon-192.png',
  './icon-512.png',
  './favicon.png',
  './css/app.css'
];

// Install event - precache essential assets
self.addEventListener('install', event => {
  event.waitUntil(
    caches.open(CACHE_NAME)
      .then(cache => cache.addAll(PRECACHE_ASSETS))
      .then(() => self.skipWaiting()) // Activate immediately
  );
});

// Activate event - clean up old caches and take control
self.addEventListener('activate', event => {
  event.waitUntil(
    caches.keys()
      .then(cacheNames => {
        return Promise.all(
          cacheNames
            .filter(cacheName => cacheName !== CACHE_NAME)
            .map(cacheName => caches.delete(cacheName))
        );
      })
      .then(() => self.clients.claim()) // Take control of all pages immediately
  );
});

// Fetch event - network-first for HTML, cache-first for assets
self.addEventListener('fetch', event => {
  const request = event.request;

  // Skip non-GET requests
  if (request.method !== 'GET') {
    return;
  }

  // Skip cross-origin requests (like CDN resources)
  if (!request.url.startsWith(self.location.origin)) {
    return;
  }

  // For navigation requests (HTML pages), try network first
  if (request.mode === 'navigate') {
    event.respondWith(
      fetch(request)
        .then(response => {
          // Cache the latest version
          const responseClone = response.clone();
          caches.open(CACHE_NAME).then(cache => cache.put(request, responseClone));
          return response;
        })
        .catch(() => caches.match(request) || caches.match(OFFLINE_URL))
    );
    return;
  }

  // For Blazor framework files (_framework/*), use cache-first with network update
  if (request.url.includes('/_framework/')) {
    event.respondWith(
      caches.match(request)
        .then(cachedResponse => {
          const fetchPromise = fetch(request)
            .then(networkResponse => {
              // Update the cache with new version
              caches.open(CACHE_NAME).then(cache => cache.put(request, networkResponse.clone()));
              return networkResponse;
            })
            .catch(() => cachedResponse);

          // Return cached version immediately, update in background
          return cachedResponse || fetchPromise;
        })
    );
    return;
  }

  // For other assets, use cache-first
  event.respondWith(
    caches.match(request)
      .then(cachedResponse => {
        if (cachedResponse) {
          return cachedResponse;
        }
        return fetch(request)
          .then(response => {
            // Cache new assets
            const responseClone = response.clone();
            caches.open(CACHE_NAME).then(cache => cache.put(request, responseClone));
            return response;
          });
      })
  );
});

// Listen for messages from the app
self.addEventListener('message', event => {
  if (event.data === 'skipWaiting') {
    self.skipWaiting();
  }
});
