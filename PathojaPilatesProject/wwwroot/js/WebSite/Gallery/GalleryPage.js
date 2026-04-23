document.addEventListener("DOMContentLoaded", () => {
    const branchChips = document.querySelectorAll("#branchChips .chip");
    const serviceChips = document.querySelectorAll("#serviceChips .chip");
    const cards = Array.from(document.querySelectorAll("#galleryGrid .gcard"));

    const visibleCount = document.getElementById("visibleCount");
    const empty = document.getElementById("galleryEmpty");
    const resetBtn = document.getElementById("resetFilters");
    const emptyReset = document.getElementById("emptyReset");

    let selectedBranch = "all";
    let selectedService = "all";

    function setActive(chips, btn) {
        chips.forEach(x => x.classList.remove("active"));
        btn.classList.add("active");
    }

    function applyFilters() {
        let shown = 0;

        cards.forEach(c => {
            const b = c.getAttribute("data-branch");
            const s = c.getAttribute("data-service");

            const okBranch = (selectedBranch === "all" || b === selectedBranch);
            const okService = (selectedService === "all" || s === selectedService);

            const show = okBranch && okService;
            c.style.display = show ? "" : "none";
            if (show) shown++;
        });

        visibleCount.textContent = String(shown);
        empty.classList.toggle("d-none", shown !== 0);
    }

    branchChips.forEach(btn => {
        btn.addEventListener("click", () => {
            selectedBranch = btn.getAttribute("data-branch");
            setActive(branchChips, btn);
            applyFilters();
        });
    });

    serviceChips.forEach(btn => {
        btn.addEventListener("click", () => {
            selectedService = btn.getAttribute("data-service");
            setActive(serviceChips, btn);
            applyFilters();
        });
    });

    function resetAll() {
        selectedBranch = "all";
        selectedService = "all";
        setActive(branchChips, document.querySelector('#branchChips .chip[data-branch="all"]'));
        setActive(serviceChips, document.querySelector('#serviceChips .chip[data-service="all"]'));
        applyFilters();
    }

    resetBtn?.addEventListener("click", resetAll);
    emptyReset?.addEventListener("click", resetAll);

    // Lightbox fill
    const modal = document.getElementById("galleryLightbox");
    const glImg = document.getElementById("glImg");
    const glTitle = document.getElementById("glTitle");
    const glDesc = document.getElementById("glDesc");

    cards.forEach(c => {
        c.addEventListener("click", () => {
            const img = c.getAttribute("data-img");
            const title = c.getAttribute("data-title") || "";
            const desc = c.getAttribute("data-desc") || "";
            glImg.src = img;
            glTitle.textContent = title;
            glDesc.textContent = desc;
        });
    });

    // Cleanup on close
    modal?.addEventListener("hidden.bs.modal", () => {
        glImg.src = "";
    });

    // init
    applyFilters();
});
