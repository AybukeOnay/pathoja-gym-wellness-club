document.addEventListener("DOMContentLoaded", function () {
    const prefersReduce = window.matchMedia("(prefers-reduced-motion: reduce)").matches;
    const items = document.querySelectorAll(".reveal-left, .reveal-right");

    // Hareket azalt tercihinde animasyonları kapat
    if (prefersReduce) {
        items.forEach(el => el.classList.add("reveal-show"));
        return;
    }

    // İlk boyamada offset'leri uygula (FOUC engellemek için)
    items.forEach(el => el.classList.add("reveal-ready"));

    // IntersectionObserver: görünür -> show, görünmez -> (once yoksa) show'u kaldır
    const io = new IntersectionObserver((entries) => {
        entries.forEach(entry => {
            const ratio = entry.intersectionRatio;

            if (ratio >= 0.20) {
                entry.target.classList.add("reveal-show");
            } else if (ratio <= 0.05) {
                if (!entry.target.classList.contains("reveal-once")) {
                    entry.target.classList.remove("reveal-show");
                }
            }
        });
    }, {
        root: null,
        threshold: [0, 0.05, 0.20, 0.8],
        rootMargin: "0px 0px -40px 0px"
    });

    items.forEach(el => io.observe(el));

    // 👉 İLK RENDER'DA da animasyon oynasın:
    // viewport içinde kalanları hemen 'reveal-show' yap (küçük gecikme ile)
    function initialReveal() {
        const vh = window.innerHeight || document.documentElement.clientHeight;
        items.forEach(el => {
            const r = el.getBoundingClientRect();
            // görünürlüğün ~%20’si kadarı içerideyse başlat
            const visible = r.top <= vh * 0.8 && r.bottom >= vh * 0.2;
            if (visible) el.classList.add("reveal-show");
        });
    }
    // CSS transition’ların hazır olabilmesi için çok kısa geciktir
    setTimeout(initialReveal, 50);
});
