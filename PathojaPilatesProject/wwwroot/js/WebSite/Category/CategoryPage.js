(function () {
    /* ===============================
       CONFIG
    =================================*/
    const STEP_MOBILE = 90;     // mobil dalga gecikmesi (ms)
    const STEP_DESKTOP = 140;   // desktop dalga gecikmesi (ms)
    const IO_THRESHOLD = 0.18;  // IntersectionObserver threshold
    const IO_ROOTMARG = '0px 0px -8% 0px';

    /* ===============================
       DOM
    =================================*/
    const grid = document.getElementById('svcGrid');
    const branchPanel = document.getElementById('branchPanel');

    /* ===============================
       ROW META (wave delay için)
    =================================*/
    function assignRowMeta(container) {
        if (!container) return;
        const cards = [...container.querySelectorAll('.card-svc')];
        const rows = [];
        const tol = 18; // aynı satır kabul toleransı (px)

        cards.forEach(el => {
            const r = el.getBoundingClientRect();
            const y = Math.round(r.top + window.scrollY);
            let row = rows.find(x => Math.abs(x.y - y) <= tol);
            if (!row) { row = { y, items: [] }; rows.push(row); }
            row.items.push(el);
        });

        rows.forEach(row => {
            row.items.sort((a, b) => a.getBoundingClientRect().left - b.getBoundingClientRect().left);
            row.items.forEach((el, idx) => {
                el.dataset.row = rows.indexOf(row).toString();
                el.dataset.col = idx.toString();
            });
        });
    }

    /* ===============================
       INTERSECTION OBSERVER
       - viewport'a girince maskeli açılış
       - tamamen çıkınca reset (tekrar oynayabilsin)
    =================================*/
    const io = new IntersectionObserver((entries) => {
        entries.forEach(en => {
            const el = en.target;
            const rect = en.boundingClientRect;
            const vh = window.innerHeight || document.documentElement.clientHeight;

            if (en.isIntersecting) {
                if (el.classList.contains('animating') || el.classList.contains('is-in')) return;

                const step = window.matchMedia('(max-width: 768px)').matches ? STEP_MOBILE : STEP_DESKTOP;
                const order = Number(el.dataset.col || 0);
                el.style.setProperty('--delay', `${order * step}ms`);

                el.classList.add('animating', 'm-reveal');
                const onEnd = () => {
                    el.classList.remove('m-reveal', 'animating');
                    el.classList.add('is-in'); // maske kalksın
                    el.removeEventListener('animationend', onEnd);
                };
                el.addEventListener('animationend', onEnd);
            } else {
                const fullyOut = rect.bottom <= 0 || rect.top >= vh;
                if (fullyOut && el.classList.contains('is-in')) {
                    el.classList.remove('is-in'); // reset
                }
            }
        });
    }, { threshold: IO_THRESHOLD, rootMargin: IO_ROOTMARG });

    /* ===============================
       BOOT
    =================================*/
    function boot() {
        if (!grid) return;

        // Kartları sırala ve gözlemle
        assignRowMeta(grid);
        grid.querySelectorAll('.card-svc').forEach(el => io.observe(el));

        // Offcanvas: linke tıklanınca paneli kapat (SSR link; sayfa değişecek)
        if (branchPanel) {
            branchPanel.querySelectorAll('.list-group-item[href]')
                .forEach(a => a.addEventListener('click', () => {
                    if (window.bootstrap?.Offcanvas) {
                        const off = bootstrap.Offcanvas.getOrCreateInstance(branchPanel);
                        off.hide();
                    }
                    // Not: SSR olduğu için sayfa yenilenecek; ekstra işlem gerekmiyor.
                }));
        }
    }

    window.addEventListener('load', boot);

    // Resize: satır/sütun order'larını yeniden hesapla
    let resizeT;
    window.addEventListener('resize', () => {
        clearTimeout(resizeT);
        resizeT = setTimeout(() => {
            if (!grid) return;
            io.disconnect();
            assignRowMeta(grid);
            grid.querySelectorAll('.card-svc').forEach(el => io.observe(el));
        }, 120);
    });
})();


document.addEventListener("DOMContentLoaded", () => {

    // Modal elementini ve içindeki parçaları yakala
    const modalEl = document.getElementById("pkgModal");
    if (!modalEl) return;

    const pkgModal = new bootstrap.Modal(modalEl);

    const pkgListEl = document.getElementById("pkgList");
    const pkgEmptyEl = document.getElementById("pkgEmpty");
    const modalTitleEl = document.getElementById("pkgModalTitle");
    const leadForm = document.getElementById("pkgLeadForm");
    const leadAlert = document.getElementById("pkgLeadAlert");
    const kvkkCheckEl = document.getElementById("kvkkCheck");
    const leadCategoryIdEl = document.getElementById("leadCategoryId");
    const leadProductIdEl = document.getElementById("leadProductId");
    const leadCategoryNameEl = document.getElementById("leadCategoryName");
    const leadLoadingEl = document.getElementById("pkgLeadLoading");
    const leadSubmitBtn = document.getElementById("pkgLeadSubmitBtn");

    const branchInput = document.getElementById("currentBranch");
    const currentBranch = branchInput ? branchInput.value : "all";

    // 1) Karttaki "Detay & Randevu" butonlarına click event bağla
    document.querySelectorAll(".btn-pkg-detail").forEach(btn => {
        btn.addEventListener("click", async (e) => {
            e.preventDefault();

            const serviceId = btn.dataset.serviceId;
            const serviceTitle = btn.dataset.serviceTitle || "Paketler";

            // Modal başlığını set et
            modalTitleEl.textContent = `${serviceTitle} için paketlerimiz`;

            // Hidden alanları doldur
            leadCategoryIdEl.value = serviceId;
            leadProductIdEl.value = "";
            leadCategoryNameEl.value = serviceTitle;// başlangıçta seçili paket yok

            // Formu ve alert'i resetle
            if (leadForm) {
                leadForm.reset();
            }
            if (leadAlert) {
                leadAlert.classList.add("d-none");
            }

            // Paketleri (Product) yükle
            await loadProductsForCategory(serviceId, currentBranch);

            // Modal'ı göster
            pkgModal.show();
        });
    });

    // 2) Category/service id'ye göre ürün/paket listesini getiren fonksiyon
    async function loadProductsForCategory(categoryId) {
        pkgListEl.innerHTML = "";
        pkgEmptyEl.classList.add("d-none");

        try {
            const url = `/Category/GetProductsForCategory?categoryId=${encodeURIComponent(categoryId)}`;
            const resp = await fetch(url);
            if (!resp.ok) throw new Error("Failed to load products");
            const data = await resp.json();

            if (!data || data.length === 0) {
                pkgEmptyEl.textContent = "Bu hizmet için tanımlı paket bulunamadı.";
                pkgEmptyEl.classList.remove("d-none");
                return;
            }

            data.forEach(p => {
                const wrap = document.createElement("div");
                wrap.className = "pkg-parent";

                const hasFeatures = Array.isArray(p.features) && p.features.length > 0;
                const hasSubProducts = Array.isArray(p.subProducts) && p.subProducts.length > 0;

                wrap.innerHTML = `
        <button type="button" class="pkg-parent-head" aria-expanded="false">
          <div class="pkg-parent-text">
            <div class="pkg-parent-title">${escapeHtml(p.name || "")}</div>
            ${p.header ? `<div class="pkg-parent-sub">${escapeHtml(p.header)}</div>` : ""}
          </div>
          <span class="pkg-parent-chevron" aria-hidden="true">▾</span>
        </button>

        <div class="pkg-parent-body" hidden>
          ${p.description ? `<div class="pkg-parent-desc">${escapeHtml(p.description)}</div>` : ""}

          ${hasFeatures ? `
            <div class="pkg-feature-grid">
              ${p.features.map(f => `
                <button type="button" class="pkg-feature" data-feature-id="${f.id}">
                  <div class="pkg-feature-top">
                    <span class="pkg-feature-count">${Number(f.totalLessonCount || 0)} ders</span>
                    <span class="pkg-feature-badge">${f.isCancellable ? "İptalli" : "İptalsiz"}</span>
                  </div>
                  <div class="pkg-feature-meta">${f.productHours ? `${Number(f.productHours)} dk` : ""}</div>
                </button>
              `).join("")}
            </div>
          ` : `
            <div class="pkg-parent-empty">Bu paket için seçenek tanımlanmamış.</div>
          `}
          ${hasSubProducts ? `
    <div class="pkg-subproducts">
    ${p.subProducts.map(sp => {
        const isHappyHours = /happy\s*hours/i.test(sp.name || "");
        return `
      <div class="pkg-subproduct-block ${isHappyHours ? "is-happy-hours" : ""}">
        <div class="pkg-subproduct-head">
          <div>
            <div class="pkg-subproduct-title">${escapeHtml(sp.name || "")}</div>
            ${sp.header ? `<div class="pkg-subproduct-sub">${escapeHtml(sp.header)}</div>` : ""}
          </div>
         
        </div>

        ${sp.description ? `<div class="pkg-subproduct-desc">${escapeHtml(sp.description)}</div>` : ""}

        ${Array.isArray(sp.features) && sp.features.length > 0 ? `
          <div class="pkg-feature-grid">
            ${sp.features.map(f => `
              <button type="button" class="pkg-feature" data-feature-id="${f.id}">
                <div class="pkg-feature-top">
                  <span class="pkg-feature-count">${Number(f.totalLessonCount || 0)} ders</span>
                  <span class="pkg-feature-badge">${f.isCancellable ? "İptalli" : "İptalsiz"}</span>
                </div>
                <div class="pkg-feature-meta">${f.productHours ? `${Number(f.productHours)} dk` : ""}</div>
              </button>
            `).join("")}
          </div>
        ` : `
          <div class="pkg-parent-empty">Bu alt paket için seçenek tanımlanmamış.</div>
        `}
      </div>
    `;
    }).join("")}
    </div>
  ` : ""}
        </div>
      `;

                const head = wrap.querySelector(".pkg-parent-head");
                const body = wrap.querySelector(".pkg-parent-body");

                // Accordion toggle (aç/kapa)
                head.addEventListener("click", () => {
                    const isOpen = head.getAttribute("aria-expanded") === "true";

                    // önce hepsini kapat
                    document.querySelectorAll(".pkg-parent").forEach(x => {
                        x.classList.remove("is-open");
                        const h = x.querySelector(".pkg-parent-head");
                        const b = x.querySelector(".pkg-parent-body");
                        if (h) h.setAttribute("aria-expanded", "false");
                        if (b) b.hidden = true;
                    });

                    // eğer zaten açıksa kapalı kalsın (toggle)
                    if (isOpen) return;

                    // aç
                    head.setAttribute("aria-expanded", "true");
                    wrap.classList.add("is-open");
                    body.hidden = false;

                    // Opsiyonel: açınca ilk feature otomatik seçilsin
                    // (İstersen bunu kapatırım)
                    const firstFeature = wrap.querySelector(".pkg-feature");
                    if (firstFeature) {
                        // sadece bu wrap içindeki seçimi temizle
                        wrap.querySelectorAll(".pkg-feature.is-selected").forEach(x => x.classList.remove("is-selected"));
                        firstFeature.classList.add("is-selected");
                        leadProductIdEl.value = firstFeature.dataset.featureId || "";
                        leadProductIdEl.value = btn.dataset.featureId || "";
                    }
                });

                // Feature select (scope: sadece o parent içinde)
                wrap.querySelectorAll(".pkg-feature").forEach(btn => {
                    btn.addEventListener("click", (e) => {
                        e.stopPropagation(); // accordion tıklamasını tetiklemesin

                        wrap.querySelectorAll(".pkg-feature.is-selected").forEach(x => x.classList.remove("is-selected"));
                        btn.classList.add("is-selected");

                        leadProductIdEl.value = firstFeature.dataset.featureId || "";
                        leadProductIdEl.value = btn.dataset.featureId || "";
                    });
                });

                pkgListEl.appendChild(wrap);
            });

        } catch (err) {
            console.error(err);
            pkgEmptyEl.textContent = "Paketler yüklenirken bir hata oluştu. Lütfen tekrar deneyin.";
            pkgEmptyEl.classList.remove("d-none");
        }
    }

    // mini helper
    function escapeHtml(str) {
        return String(str).replace(/[&<>"']/g, s => ({
            "&": "&amp;", "<": "&lt;", ">": "&gt;", '"': "&quot;", "'": "&#039;"
        }[s]));
    }
    //Talep bekletme ekranı
    function setLeadLoading(isLoading) {
        if (leadLoadingEl) {
            leadLoadingEl.classList.toggle("d-none", !isLoading);
        }

        if (leadSubmitBtn) {
            leadSubmitBtn.disabled = isLoading;
        }

        if (leadForm) {
            const fields = leadForm.querySelectorAll("input, button, textarea, select");
            fields.forEach(el => {
                if (isLoading) {
                    el.setAttribute("disabled", "disabled");
                } else {
                    el.removeAttribute("disabled");
                }
            });
        }
    }

    const fullNameInput = leadForm.querySelector('input[name="FullName"]');
    const phoneInput = leadForm.querySelector('input[name="Phone"]');
    const emailInput = leadForm.querySelector('input[name="Email"]');

    if (fullNameInput) {
        fullNameInput.addEventListener("invalid", function () {
            if (this.validity.valueMissing) {
                this.setCustomValidity("Lütfen adınız ve soyadınızı giriniz.");
            } else {
                this.setCustomValidity("");
            }
        });
        fullNameInput.addEventListener("input", function () {
            this.setCustomValidity("");
        });
    }

    if (phoneInput) {
        // İstersen pattern de koyabiliriz
        phoneInput.setAttribute("pattern", "^[0-9]{10,11}$");

        phoneInput.addEventListener("invalid", function () {
            if (this.validity.valueMissing) {
                this.setCustomValidity("Lütfen telefon numaranızı giriniz.");
            } else if (this.validity.patternMismatch) {
                this.setCustomValidity("Lütfen geçerli bir telefon numarası giriniz.");
            } else {
                this.setCustomValidity("");
            }
        });
        phoneInput.addEventListener("input", function () {
            let digits = phoneInput.value.replace(/\D/g, "");
            if (digits.length > 11) {
                digits = digits.slice(0, 11);
            }
            phoneInput.value = digits;
            this.setCustomValidity("");
        });
    }

    if (emailInput) {
        const emailPattern = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;

        emailInput.addEventListener("invalid", function () {
            if (this.validity.valueMissing) {
                this.setCustomValidity("Lütfen e-posta adresinizi giriniz.");
            } else if (this.validity.typeMismatch || !emailPattern.test(this.value) ) {
                this.setCustomValidity("Lütfen geçerli bir e-posta adresi giriniz (ör. ornek@mail.com).");
            } else {
                this.setCustomValidity("");
            }
        });
        emailInput.addEventListener("input", function () {
            // Boşken tarayıcı kendi required kontrolünü yapacak, o yüzden sil
            if (!this.value) {
                this.setCustomValidity("");
                return;
            }

            if (!emailPattern.test(this.value)) {
                this.setCustomValidity(
                    "Lütfen geçerli bir e-posta adresi giriniz (ör. ornek@mail.com)."
                );
            } else {
                this.setCustomValidity("");
            }
        });
    }

    if (kvkkCheckEl) {
        kvkkCheckEl.addEventListener("invalid", function () {
            if (this.validity.valueMissing) {
                this.setCustomValidity("KVKK onayı vermeden talep oluşturulamaz.");
            } else {
                this.setCustomValidity("")
            }
        });

        kvkkCheckEl.addEventListener("change", function () {
            this.setCustomValidity("");
        });
    }

    // 3) Form submit → lead isteği gönder
    if (leadForm) {
        leadForm.addEventListener("submit", async (e) => {
            e.preventDefault();

            const formData = new FormData(leadForm);
            const kvkkApproved = kvkkCheckEl ? kvkkCheckEl.checked : false;

            const payload = {
                categoryId: parseInt(leadCategoryIdEl.value || "0"),
                productId: leadProductIdEl.value ? parseInt(leadProductIdEl.value) : null,
                categoryName: leadCategoryNameEl.value,
                fullName: formData.get("FullName"),
                phone: formData.get("Phone"),
                email: formData.get("Email"),
                note: formData.get("Note"),
                kvkkApproved: kvkkApproved
            };

            setLeadLoading(true);

            try {
                const resp = await fetch("/Category/RequestProductLead", {
                    method: "POST",
                    headers: { "Content-Type": "application/json" },
                    body: JSON.stringify(payload)
                });

                if (!resp.ok) {
                    let msg = "Talebiniz gönderilirken bir hata oluştu. Lütfen tekrar deneyin.";

                    try {
                        const data = await resp.json();
                        if (data && data.message) {
                            msg = data.message;
                        }
                    } catch { }

                    showToast(msg, "error");
                    return;
                }

                leadForm.reset();
                if (kvkkCheckEl) kvkkCheckEl.checked = false;

                showToast("Talebiniz alınmıştır. En kısa sürede sizinle iletişime geçeceğiz.", "success");
                setTimeout(() => {
                    pkgModal.hide();
                }, 250);
            } catch (err) {
                console.error(err);
                showToast("Talebiniz gönderilirken bir hata oluştu. Lütfen tekrar deneyin.", "error");
            } finally {
                setLeadLoading(false);
            }
        });
    }
});


//BRANCH PANELİ
document.addEventListener("DOMContentLoaded", function () {
    const chip = document.getElementById("branchChip");
    const panelEl = document.getElementById("branchPanel");

    if (!chip || !panelEl) return;

    const isDesktop = () => window.matchMedia("(min-width: 768px)").matches;

    function positionBranchPanel() {
        if (!isDesktop()) return;

        const rect = chip.getBoundingClientRect();
        const panelWidth = 340;        // CSS ile aynı
        const margin = 16;
        const gap = 12;

        // Panel butonun SAĞINA açılsın; ama ekrandan taşarsa sola kaydır
        let left = rect.right + gap;
        left = Math.min(left, window.innerWidth - panelWidth - margin);
        left = Math.max(margin, left);

        // Top: butonun üst hizasına yakın dursun (istersen rect.top yerine rect.bottom da kullanabiliriz)
        let top = rect.top - 8;
        top = Math.max(margin, Math.min(top, window.innerHeight - 260)); // 260 ~ min panel görünür alan

        panelEl.style.setProperty("--bp-left", `${left}px`);
        panelEl.style.setProperty("--bp-top", `${top}px`);
    }

    // Offcanvas açılmadan hemen önce/sonra konumla
    panelEl.addEventListener("show.bs.offcanvas", positionBranchPanel);
    panelEl.addEventListener("shown.bs.offcanvas", positionBranchPanel);

    // Açıkken scroll/resize olursa konumu güncelle
    window.addEventListener("resize", () => {
        if (panelEl.classList.contains("show")) positionBranchPanel();
    });

    // Sayfa içi scroll (özellikle container scroll) için:
    window.addEventListener("scroll", () => {
        if (panelEl.classList.contains("show")) positionBranchPanel();
    }, true);
});


