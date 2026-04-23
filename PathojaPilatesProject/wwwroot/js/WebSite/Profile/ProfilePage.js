// Profil tabs: URL hash + localStorage ile aktif sekmeyi hatırla
(function () {
    const key = "profile-active-tab";
    const tabsEl = document.querySelector('#profileTabs');
    if (!tabsEl) return;

    // hash veya localStorage'dan aç
    const hash = location.hash || localStorage.getItem(key);
    if (hash) {
        const trigger = document.querySelector(`[href="${hash}"][data-bs-toggle="tab"]`);
        if (trigger) new bootstrap.Tab(trigger).show();
    }

    // her geçişte kaydet
    document.querySelectorAll('[data-bs-toggle="tab"]').forEach(el => {
        el.addEventListener('shown.bs.tab', e => {
            const href = e.target.getAttribute('href');
            localStorage.setItem(key, href);
            if (history.replaceState) history.replaceState(null, "", href);
        });
    });
})();
