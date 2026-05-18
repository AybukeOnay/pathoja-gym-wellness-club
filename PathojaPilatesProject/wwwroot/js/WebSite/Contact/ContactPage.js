// contact-branches.js
document.addEventListener("DOMContentLoaded", () => {
    const tabsContainer = document.getElementById("branchTabs");
    if (!tabsContainer) return;

    // Detay paneli elementleri (ID'ler değişmedi)
    const nameEl = document.getElementById("dName");
    const addrEl = document.getElementById("dAddress");
    const phoneEl = document.getElementById("dPhone");
    const mailEl = document.getElementById("dEmail");
    const mapEl = document.getElementById("branchMap");
    const qrImg = document.getElementById("branchQr");

    // Bir <li class="branch-tab">'den meta bilgiyi oku
    function getMetaFromLi(li) {
        return {
            id: li.dataset.id || "",
            name: (li.dataset.name || "").trim(),
            address: (li.dataset.address || "").trim(),
            phoneNumber: (li.dataset.phone || "").trim(),
            email: (li.dataset.email || "").trim(),
            lat: li.dataset.lat ? Number(li.dataset.lat) : null,
            lng: li.dataset.lng ? Number(li.dataset.lng) : null,
            mapsUrl: (li.dataset.mapsUrl || "").trim()
        };
    }

    // Sol paneli güncelle
    function setDetails(meta) {
        if (nameEl) nameEl.textContent = meta.name || "";
        if (addrEl) {
            if (meta.address) {
                let href = "";

                if (meta.mapsUrl) {
                    href = meta.mapsUrl;
                } else if (meta.lat != null && meta.lng != null && !Number.isNaN(meta.lat) && !Number.isNaN(meta.lng)) {
                    href = `https://www.google.com/maps?q=${meta.lat},${meta.lng}`;
                } else {
                    const query = [meta.name, meta.address].filter(Boolean).join(" - ").trim();
                    href = `https://www.google.com/maps/search/?api=1&query=${encodeURIComponent(query)}`;
                }

                addrEl.innerHTML = `<a href="${href}" target="_blank" rel="noopener noreferrer">${meta.address}</a>`;
                addrEl.style.display = "";
            } else {
                addrEl.innerHTML = "";
                addrEl.style.display = "none";
            }
        }

        if (phoneEl) {
            if (meta.phoneNumber) {
                const tel = meta.phoneNumber.replace(/\s+/g, "");
                phoneEl.innerHTML = `<a href="tel:${tel}">${meta.phoneNumber}</a>`;
                phoneEl.style.display = "";
            } else {
                phoneEl.innerHTML = "";
                phoneEl.style.display = "none";
            }
        }

        if (mailEl) {
            if (meta.email) {
                mailEl.innerHTML = `<a href="mailto:${meta.email}">${meta.email}</a>`;
                mailEl.style.display = "";
            } else {
                mailEl.innerHTML = "";
                mailEl.style.display = "none";
            }
        }
    }

    // mapsUrl'den iframe için kullanılabilir bilgi çıkarmaya çalış
    function parseMapSource(meta) {
        const result = {
            type: "fallback", // "coords" | "embed" | "query" | "fallback"
            value: ""
        };

        // 1) Lat/Lng varsa direkt en sağlam yöntem
        const hasValidLatLng =
            meta.lat != null &&
            meta.lng != null &&
            !Number.isNaN(meta.lat) &&
            !Number.isNaN(meta.lng);

        if (hasValidLatLng) {
            result.type = "coords";
            result.value = `${meta.lat},${meta.lng}`;
            return result;
        }

        // 2) mapsUrl varsa parse etmeyi dene
        if (meta.mapsUrl) {
            try {
                const url = new URL(meta.mapsUrl);

                const host = (url.hostname || "").toLowerCase();
                const path = url.pathname || "";

                // 2.a) Zaten embed link ise direkt kullan
                // örn: https://www.google.com/maps/embed?...
                if ((host.includes("google.com") || host.includes("google.") || host.includes("maps.google")) &&
                    path.includes("/maps/embed")) {
                    result.type = "embed";
                    result.value = meta.mapsUrl;
                    return result;
                }

                // 2.b) maps.app.goo.gl short link -> iframe için güvenilmez (redirect/app intent)
                if (host.includes("maps.app.goo.gl")) {
                    // burada exact çözemeyiz, fallback'e bırakıyoruz
                } else {
                    // 2.c) q / query paramları
                    const qParam = url.searchParams.get("q");
                    if (qParam && qParam.trim()) {
                        result.type = "query";
                        result.value = qParam.trim();
                        return result;
                    }

                    const queryParam = url.searchParams.get("query");
                    if (queryParam && queryParam.trim()) {
                        result.type = "query";
                        result.value = queryParam.trim();
                        return result;
                    }

                    // 2.d) URL içinde @lat,lng yakala (çok işe yarar)
                    // örn: .../@39.90,32.80,17z
                    const atMatch = meta.mapsUrl.match(/@(-?\d+(?:\.\d+)?),(-?\d+(?:\.\d+)?)/);
                    if (atMatch) {
                        result.type = "coords";
                        result.value = `${atMatch[1]},${atMatch[2]}`;
                        return result;
                    }

                    // 2.e) /maps/place/... path'inden place adı çek
                    const placeMatch = path.match(/\/maps\/place\/([^/]+)/i);
                    if (placeMatch && placeMatch[1]) {
                        const placeText = decodeURIComponent(placeMatch[1].replace(/\+/g, " ")).trim();
                        if (placeText) {
                            result.type = "query";
                            result.value = placeText;
                            return result;
                        }
                    }
                }
            } catch {
                // geçersiz URL ise sessizce fallback
            }
        }

        // 3) Fallback (arama) -> name + address birlikte daha iyi sonuç verir
        const fallbackQuery = [meta.name, meta.address].filter(Boolean).join(" - ").trim();
        result.type = "fallback";
        result.value = fallbackQuery;
        return result;
    }

    // Sağdaki iframe haritayı güncelle
    function setMap(meta) {
        if (!mapEl) return;

        const parsed = parseMapSource(meta);

        if (parsed.type === "embed") {
            mapEl.src = parsed.value;
            return;
        }

        // coords / query / fallback -> q ile embed
        mapEl.src = `https://www.google.com/maps?q=${encodeURIComponent(parsed.value)}&hl=tr&z=16&output=embed`;
    }

    // QR'ı güncelle
    function setQr(meta) {
        if (!qrImg) return;

        const baseUrl = qrImg.dataset.baseUrl; // /Contact/LocationQr
        if (!baseUrl) return;

        // branchId parametresini değiştir
        qrImg.src = `${baseUrl}?branchId=${encodeURIComponent(meta.id)}`;
    }

    // Aktif tab'ı işaretle + detayları/haritayı bas
    function activateTab(li) {
        tabsContainer.querySelectorAll(".branch-tab").forEach(x => x.classList.remove("active"));
        li.classList.add("active");

        const meta = getMetaFromLi(li);

        setDetails(meta);
        setMap(meta);
        setQr(meta);

        // Geçici debug için açabilirsin:
        // console.log("Branch meta:", meta);
        // console.log("Map src:", mapEl?.src);
    }

    // Tıklama
    tabsContainer.addEventListener("click", (e) => {
        const li = e.target.closest(".branch-tab");
        if (!li) return;
        activateTab(li);
    });

    // İlk açılışta aktif veya ilk li'yi yükle
    const firstActive =
        tabsContainer.querySelector(".branch-tab.active") ||
        tabsContainer.querySelector(".branch-tab");

    if (firstActive) activateTab(firstActive);
});

document.addEventListener("DOMContentLoaded", function () {
    const phoneInput = document.querySelector("input[name='Phone']");

    if (!phoneInput) return;

    phoneInput.addEventListener("input", function () {
        this.value = this.value.replace(/\D/g, "").slice(0, 10);
    });

    
});

// Contact form submit lock
document.addEventListener("DOMContentLoaded", () => {
    const form = document.getElementById("contactForm");
    const submitBtn = document.getElementById("contactSubmitBtn");
    const overlay = document.getElementById("contactSubmitOverlay");

    if (!form || !submitBtn) return;

    let isSubmitting = false;

    form.addEventListener("submit", function (e) {
        if (isSubmitting) {
            e.preventDefault();
            return;
        }

        isSubmitting = true;

        submitBtn.disabled = true;

        const btnText = submitBtn.querySelector(".btn-text");
        const btnLoading = submitBtn.querySelector(".btn-loading");

        if (btnText) btnText.classList.add("d-none");
        if (btnLoading) btnLoading.classList.remove("d-none");

        if (overlay) {
            overlay.classList.remove("d-none");
        }
    });
});