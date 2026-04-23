document.addEventListener("DOMContentLoaded", function () {
    setupTagSelect("CategoryIds", "selectedCategories");
    setupTagSelect("BranchIds", "selectedBranches");
    const passwordInput = document.getElementById("passwordInput");
    const togglePassword = document.getElementById("togglePassword");


    if (passwordInput && togglePassword) {
        togglePassword.style.display = "none"; // Başta gizli kalmasını sağla

        passwordInput.addEventListener("input", function () {
            if (this.value.length > 0) {
                togglePassword.classList.add("visible");
                togglePassword.style.display = "block";
            } else {
                togglePassword.classList.remove("visible");
                togglePassword.style.display = "none";
            }
        });

        togglePassword.addEventListener("click", function () {
            const isPassword = passwordInput.getAttribute("type") === "password";
            passwordInput.setAttribute("type", isPassword ? "text" : "password");
            togglePassword.innerHTML = isPassword
                ? '<i class="fas fa-eye-slash"></i>'
                : '<i class="fas fa-eye"></i>';
        });
    }
});

function setupTagSelect(selectId, containerId) {
    const selectElement = document.getElementById(selectId);
    const container = document.getElementById(containerId);

    if (!selectElement || !container) {
        console.warn(`Element bulunamadı: ${selectId} veya ${containerId}`);
        return;
    }

    function updateTags() {
        container.innerHTML = "";

        Array.from(selectElement.options).forEach(option => {
            if (option.selected) {
                const tag = document.createElement("div");
                tag.classList.add("tag-item");
                tag.innerHTML = `
                    <span>${option.text}</span>
                    <button type="button" class="remove" aria-label="${option.text} seçimini kaldır">×</button>
                `;

                // chip içindeki X butonuna basıldığında
                tag.querySelector(".remove").addEventListener("click", function () {
                    option.selected = false;
                    option.hidden = false;
                    updateTags();
                });

                container.appendChild(tag);
                option.hidden = true;
            } else {
                option.hidden = false;
            }
        });
    }

    // ✨ Seçim yapıldığında doğrudan updateTags çağır
    selectElement.addEventListener("mousedown", (e) => {
        e.preventDefault(); // tarayıcının default seç/deselect işini engelle
        const option = e.target;
        if (option.tagName.toLowerCase() === "option") {
            option.selected = !option.selected; // toggle yap
            updateTags();
        }
    });

    // Container tıklanınca select açılabilsin
    container.addEventListener("click", () => {
        selectElement.focus();
    });

    // İlk yüklemede chip’leri çiz
    updateTags();
}

function previewImage(event) {
    const fileInput = event.target;
    const preview = document.getElementById("preview");
    const previewContainer = document.getElementById("preview-container");

    if (fileInput.files && fileInput.files[0]) {
        const reader = new FileReader();
        reader.onload = function (e) {
            preview.src = e.target.result;
            previewContainer.style.display = "flex"; // sadece seçim olduğunda görünsün
            previewContainer.style.alignItems = "center";
        }
        reader.readAsDataURL(fileInput.files[0]);
    } else {
        // Eğer seçim iptal edilirse yine gizle
        previewContainer.style.display = "none";
    }
}

function removeImage() {
    const fileInput = document.getElementById("ImageFileInput");
    const previewContainer = document.getElementById("preview-container");

    fileInput.value = "";          // input’u sıfırla
    previewContainer.style.display = "none"; // preview ve buton kaybolsun
}



