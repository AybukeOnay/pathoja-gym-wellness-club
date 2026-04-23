document.addEventListener("DOMContentLoaded", function () {
    const hero = document.getElementById("hero-section");
    if (!hero) return;

    const slides = hero.querySelectorAll(".hero-slide");
    const dotsWrap = hero.querySelector("#hero-dots");
    const prevBtn = hero.querySelector(".hero-arrow-prev");
    const nextBtn = hero.querySelector(".hero-arrow-next");

    let currentIndex = 0;
    let timerId = null;
    let isAnimating = false;
    const INTERVAL = 5000;
    const FADE_DURATION = 700; // CSS transition ile uyumlu

    if (!slides.length || !dotsWrap) return;

    // güvenlik: script tekrar çalışırsa dot birikmesin
    dotsWrap.innerHTML = "";

    // dots oluştur
    slides.forEach((_, i) => {
        const d = document.createElement("button");
        d.type = "button";
        d.className = "dot" + (i === 0 ? " active" : "");
        d.setAttribute("data-index", String(i));
        d.setAttribute("aria-label", `${i + 1}. görsel`);
        dotsWrap.appendChild(d);
    });

    const dots = dotsWrap.querySelectorAll(".dot");

    function setActiveSlide(index) {
        if (isAnimating) return;
        if (index === currentIndex) return;

        isAnimating = true;

        slides[currentIndex].classList.remove("active");
        dots[currentIndex].classList.remove("active");

        currentIndex = index;

        slides[currentIndex].classList.add("active");
        dots[currentIndex].classList.add("active");

        setTimeout(() => {
            isAnimating = false;
        }, FADE_DURATION);
    }

    function next() {
        const nextIdx = (currentIndex + 1) % slides.length;
        setActiveSlide(nextIdx);
    }

    function prev() {
        const prevIdx = (currentIndex - 1 + slides.length) % slides.length;
        setActiveSlide(prevIdx);
    }

    function stop() {
        if (timerId) {
            clearInterval(timerId);
            timerId = null;
        }
    }

    function start() {
        stop(); // en kritik satır: birikmeyi engeller
        timerId = setInterval(() => {
            if (!isAnimating) {
                next();
            }
        }, INTERVAL);
    }

    function restart() {
        start();
    }

    dots.forEach(dot => {
        dot.addEventListener("click", (e) => {
            const idx = parseInt(e.currentTarget.getAttribute("data-index"), 10);
            setActiveSlide(idx);
            restart();
        });
    });

    if (nextBtn) {
        nextBtn.addEventListener("click", () => {
            if (isAnimating) return;
            next();
            restart();
        });
    }

    if (prevBtn) {
        prevBtn.addEventListener("click", () => {
            if (isAnimating) return;
            prev();
            restart();
        });
    }

    hero.addEventListener("mouseenter", stop);
    hero.addEventListener("mouseleave", start);

    start();
});

// WHY US — Scroll Reveal + CountUp + Letter animation
(function () {
    const io = new IntersectionObserver((entries) => {
        entries.forEach(e => {
            if (e.isIntersecting) {
                e.target.classList.add('reveal-active');

                // sayıları çalıştır
                if (e.target.classList.contains('whyus-card')) {
                    const num = e.target.querySelector('.countup');
                    if (num && !num.dataset.done) {
                        animateCount(num, parseInt(num.dataset.to, 10) || 0, 900);
                        num.dataset.done = "1";
                    }
                }
                // bir kere yeter
                io.unobserve(e.target);
            }
        });
    }, { threshold: .2 });

    // data-reveal etiketlerini bağla
    document.querySelectorAll('[data-reveal]').forEach(el => io.observe(el));
    document.querySelectorAll('.whyus-card').forEach(el => io.observe(el));

    // Başlığı harf harf yumuşak geçiş (büyük başlıkta .stroke kelimesi)
    const stroke = document.querySelector('.whyus-title .stroke');
    if (stroke) {
        const text = stroke.textContent.trim();
        stroke.textContent = '';
        [...text].forEach((ch, i) => {
            const span = document.createElement('span');
            span.textContent = ch;
            span.style.display = 'inline-block';
            span.style.transform = 'translateY(20px)';
            span.style.opacity = '0';
            span.style.transition = 'transform .7s cubic-bezier(.2,.8,.2,1), opacity .7s';
            span.style.transitionDelay = (0.05 * i) + 's';
            stroke.appendChild(span);
        });

        // Göründüğünde tetikle
        const ioH = new IntersectionObserver((entries) => {
            entries.forEach(e => {
                if (e.isIntersecting) {
                    stroke.querySelectorAll('span').forEach(s => {
                        requestAnimationFrame(() => { s.style.transform = 'none'; s.style.opacity = '1'; });
                    });
                    ioH.unobserve(stroke);
                }
            });
        }, { threshold: .6 });
        ioH.observe(stroke);
    }

    // Basit countup
    function animateCount(el, to, dur) {
        const from = 0;
        const start = performance.now();
        const formatter = new Intl.NumberFormat('tr-TR');
        function tick(now) {
            const p = Math.min(1, (now - start) / dur);
            const ease = 1 - Math.pow(1 - p, 3);
            const val = Math.round(from + (to - from) * ease);
            el.textContent = formatter.format(val);
            if (p < 1) requestAnimationFrame(tick);
        }
        requestAnimationFrame(tick);
    }
})();


// SERVICES  
(function () {
    const section = document.getElementById('services');
    if (!section) return;

    const leftItems  = Array.from(section.querySelectorAll('.services-list-left li'));
    const rightItems = Array.from(section.querySelectorAll('.services-list-right li'));

    /* --- 1) IntersectionObserver: ilk giriş animasyonu --- */
    const observer = new IntersectionObserver((entries) => {
        entries.forEach(entry => {
            if (entry.target !== section) return;

            if (entry.isIntersecting) {
                section.classList.add('is-inview');
            } else {
                section.classList.remove('is-inview');
            }
        });
    }, {
        threshold: 0.3   // section'ın %30'u görünür olunca tetikle
    });

    observer.observe(section);

    /* --- 2) Scroll ile sürekli sağ/sol kayma --- */

    function clamp(v, min, max) {
        return Math.min(max, Math.max(min, v));
    }

    function updateSlide() {
        const rect = section.getBoundingClientRect();
    const vh   = window.innerHeight || document.documentElement.clientHeight;

    // Section ekrandan tamamen çıktıysa hiç uğraşma
    if (rect.bottom < 0 || rect.top > vh) {
            return;
        }

    // Section merkezinin ekrana göre konumu (-1 .. 1 arası)
    const center = rect.top + rect.height / 2;
    const progress = clamp((vh / 2 - center) / (vh / 2), -1, 1);

        // Sol taraftakiler: soldan → merkeze
        leftItems.forEach((el, index) => {
            const base = 70 + index * 10;      // her satır için biraz farklı güç
    const x = -base * (1 - Math.abs(progress)); 
    // progress 0'a yaklaştıkça x -> 0, yukarı/aşağı giderken tekrar açılıyor

    el.style.transform = 'translateX(' + x + 'px)';
        });

        // Sağ taraftakiler: sağdan → merkeze
        rightItems.forEach((el, index) => {
            const base = 70 + index * 10;
    const x = base * (1 - Math.abs(progress));

    el.style.transform = 'translateX(' + x + 'px)';
        });
    }

    window.addEventListener('scroll', updateSlide);
    window.addEventListener('resize', updateSlide);

    // Bilerek "load" anında çağırmıyoruz ki ilk pozisyonu tarayıcı çizsin,
    // kullanıcı 1px bile scroll yaptığında animasyon devreye girsin.
})();

//TESTIMONIALS - ÜYE YORUMLARII
(function () {
    const section = document.getElementById('testimonials');
    if (!section) return;

    const track  = section.querySelector('.testimonials-track');
    const cards  = Array.from(section.querySelectorAll('.testimonial-card'));
    const prev   = section.querySelector('.t-nav-prev');
    const next   = section.querySelector('.t-nav-next');
    const dotsWrap = section.querySelector('.testimonials-dots');

    let index = 0;
    let autoTimer = null;

    // --- Dots oluştur ---
    const dots = cards.map((_, i) => {
        const btn = document.createElement('button');
    if (i === 0) btn.classList.add('is-active');
        btn.addEventListener('click', () => goTo(i, true));
    dotsWrap.appendChild(btn);
    return btn;
    });

    function updateUI() {
        const offset = -index * 100;
    track.style.transform = 'translateX(' + offset + '%)';

        dots.forEach((d, i) => {
        d.classList.toggle('is-active', i === index);
        });
    }

    function goTo(i, stopAuto) {
        const total = cards.length;
    if (i < 0) i = total - 1;
        if (i >= total) i = 0;
    index = i;
    updateUI();

    if (stopAuto) {
        resetAuto();
        }
    }

    function nextSlide() {
        goTo(index + 1, false);
    }

    function resetAuto() {
        if (autoTimer) clearInterval(autoTimer);
    autoTimer = setInterval(nextSlide, 6000); // 6 sn'de bir otomatik geçiş
    }

    // Ok eventleri
    if (prev) prev.addEventListener('click', () => goTo(index - 1, true));
    if (next) next.addEventListener('click', () => goTo(index + 1, true));

    // Hover'da autoplay dursun, çıkınca devam etsin
    section.addEventListener('mouseenter', () => {
        if (autoTimer) clearInterval(autoTimer);
    });

    section.addEventListener('mouseleave', () => {
        resetAuto();
    });

    // Başlat
    updateUI();
    resetAuto();
})();


//SAYFALARDA YUKARI ÇIKMA OK BUTON İŞLEMİ
(function () {
    const btn = document.getElementById('scrollTopBtn');
if (!btn) return;

function toggleScrollTop() {
        if (window.scrollY > 250) {    // 250px aşağı inince görünsün
    btn.classList.add('show');
        } else {
    btn.classList.remove('show');
        }
    }

// scroll’da görünürlük kontrolü
window.addEventListener('scroll', toggleScrollTop);
toggleScrollTop();

// tıklanınca yumuşak şekilde en üste git
btn.addEventListener('click', function () {
    window.scrollTo({
        top: 0,
        behavior: 'smooth'
    });
    });
})();

document.addEventListener("DOMContentLoaded", function () {
    const navCollapse = document.querySelector(".main-navbar .navbar-collapse");
    const toggler = document.querySelector(".custom-toggler");

    if (!navCollapse || !toggler) return;

    function hideMobileMenu() {
        if (window.innerWidth > 991.98) return;
        if (!navCollapse.classList.contains("show")) return;

        const bsCollapse =
            bootstrap.Collapse.getInstance(navCollapse) ||
            new bootstrap.Collapse(navCollapse, { toggle: false });

        bsCollapse.hide();
    }

    // Menü dışına tıklanınca kapat
    document.addEventListener("click", function (e) {
        if (window.innerWidth > 991.98) return;

        const clickedInsideMenu = navCollapse.contains(e.target);
        const clickedToggler = toggler.contains(e.target);

        if (!clickedInsideMenu && !clickedToggler) {
            hideMobileMenu();
        }
    });

    // Scroll olunca kapat
    document.addEventListener("scroll", function () {
        hideMobileMenu();
    }, true);

    // Menü linkine basınca kapat
    navCollapse.querySelectorAll("a.nav-link, a.nav-link-main").forEach(link => {
        link.addEventListener("click", function () {
            hideMobileMenu();
        });
    });
});


