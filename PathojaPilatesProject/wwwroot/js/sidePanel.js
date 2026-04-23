function openSidePanel(panelId) {
    document.getElementById(panelId).classList.add("open");
    document.getElementById("overlay-backdrop").classList.add("active");
    document.body.style.overflow = "hidden"; // scroll kapalı
}

function closeSidePanel(panelId) {
    document.getElementById(panelId).classList.remove("open");
    document.getElementById("overlay-backdrop").classList.remove("active");
    document.body.style.overflow = ""; // scroll geri açılır
}

document.addEventListener("keydown", function (e) {
    if (e.key === "Escape") {
        const openPanel = document.querySelector(".sidepanel.open");
        if (openPanel) {
            closeSidePanel(openPanel.id);
        }
    }
});