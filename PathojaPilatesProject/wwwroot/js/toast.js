function showToast(message, type = "success") {
    // Container yoksa ekle
    let container = document.querySelector(".custom-toast-container");
    if (!container) {
        container = document.createElement("div");
        container.className = "custom-toast-container";
        document.body.appendChild(container);
    }

    // İkon seçimi
    let icon = "";
    if (type === "success") icon = '<i class="fa fa-check-circle"></i>';
    if (type === "warning") icon = '<i class="fa fa-exclamation-circle"></i>';
    if (type === "error") icon = '<i class="fa fa-times-circle"></i>';

    // Toast elementi oluştur
    const toast = document.createElement("div");
    toast.className = `custom-toast ${type}`;
    toast.innerHTML = `${icon} <span>${message}</span>`;

    // Container’a ekle
    container.appendChild(toast);

    // 6 saniye sonra sil
    setTimeout(() => {
        toast.remove();
    },6000);
}
