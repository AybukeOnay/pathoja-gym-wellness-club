document.addEventListener("DOMContentLoaded", function () {
    const cards = document.querySelectorAll(".teacher-card");
    const editBtn = document.getElementById("btn-edit");
    const deleteBtn = document.getElementById("btn-delete");

    let selectedCardId = null;

    // 📌 Kart seçimi
    cards.forEach(card => {
        card.addEventListener("click", function () {
            if (card.classList.contains("selected")) {
                card.classList.remove("selected");
                selectedCardId = null;

                editBtn.classList.add("disabled");                
                deleteBtn.classList.add("disabled");

                editBtn.removeEventListener("click", handleEditClick);
                deleteBtn.href = "#";
            } else {
                cards.forEach(c => c.classList.remove("selected"));

                card.classList.add("selected");
                selectedCardId = card.getAttribute("data-id");

                editBtn.classList.remove("disabled");
                deleteBtn.classList.remove("disabled");

                editBtn.addEventListener("click", handleEditClick);
            }
        });
    });
    function handleEditClick(e) {
        e.preventDefault();
        if (!selectedCardId) return;

        fetch(`/Teacher/EditTeacher?id=${selectedCardId}`)
            .then(res => res.text())
            .then(html => {
                document.getElementById("editTeacherPanelBody").innerHTML = html;
                openSidePanel("editTeacherPanel");

                initSelect2();
                bindEditFormSubmit(); // 🔹 Form submit’i ayrı fonksiyona çektik
            })
            .catch(err => console.error("Edit panel yüklenemedi:", err));
    }

    function initSelect2() {
        $('#CategoryIds').select2({
            placeholder: "Kategori seçiniz",
            allowClear: true,
            width: '100%',
            language: {
                searching: () => "Aranıyor...",
                noResults: () => "Sonuç bulunamadı"
            }
        });

        $('#BranchIds').select2({
            placeholder: "Branş seçiniz",
            allowClear: true,
            width: '100%',
            language: {
                searching: () => "Aranıyor...",
                noResults: () => "Sonuç bulunamadı"
            }
        });
    }

    function bindEditFormSubmit() {
        const form = document.querySelector("#edit-teacher-form");
        if (!form) return;

        form.addEventListener("submit", function (e) {
            e.preventDefault();

            const formData = new FormData(form);

            fetch("/Teacher/EditTeacher", {
                method: "POST",
                body: formData
            })
                .then(res => res.text())
                .then(html => {
                    try {
                        const data = JSON.parse(html);
                        if (data.success) {
                            closeSidePanel("editTeacherPanel");
                            window.location.reload();
                            return;
                        }
                    } catch {
                        // 🔹 Validation hatası varsa tekrar partial yüklenir
                        document.getElementById("editTeacherPanelBody").innerHTML = html;
                        initSelect2();
                        bindEditFormSubmit(); // 🔹 Yeni gelen form’a tekrar event ekle
                    }
                });
        });
    }

    //function handleEditClick(e) {
    //    e.preventDefault();
    //    if (!selectedCardId) return;

    //    fetch(`/Teacher/EditTeacher?id=${selectedCardId}`)
    //        .then(res => res.text())
    //        .then(html => {
    //            document.getElementById("editTeacherPanelBody").innerHTML = html;
    //            openSidePanel("editTeacherPanel");

    //            $('#CategoryIds').select2({
    //                placeholder: "Kategori seçiniz",
    //                allowClear: true,
    //                width: '100%',
    //                language: {
    //                    searching: () => "Aranıyor...",
    //                    noResults: () => "Sonuç bulunamadı"
    //                }
    //            });

    //            $('#BranchIds').select2({
    //                placeholder: "Branş seçiniz",
    //                allowClear: true,
    //                width: '100%',
    //                language: {
    //                    searching: () => "Aranıyor...",
    //                    noResults: () => "Sonuç bulunamadı"
    //                }
    //            });

    //            //FORM-SUBMİT
    //            const form = document.querySelector("#edit-teacher-form");
    //            if (form) {
    //                form.addEventListener("submit", function (e) {                     
    //                        e.preventDefault();
                
    //                        const formData = new FormData(form);

    //                        fetch("/Teacher/EditTeacher", {
    //                            method: "POST",
    //                            body: formData
    //                        })
    //                            .then(res => res.text())
    //                            .then(html => {
    //                                // Eğer JSON dönmüşse (success:true) → paneli kapat
    //                                try {
    //                                    const data = JSON.parse(html);
    //                                    if (data.success) {
    //                                        closeSidePanel("editTeacherPanel");
    //                                        window.location.reload();
    //                                        return;
    //                                    }
    //                                } catch {
    //                                    // JSON parse edilemediyse → Partial dönmüştür (hatalar var)
    //                                    document.getElementById("editTeacherPanelBody").innerHTML = html;

    //                                    // tekrar select2 initialize et
    //                                    $('#CategoryIds').select2({ width: "100%" });
    //                                    $('#BranchIds').select2({ width: "100%" });
    //                                }
    //                            });
    //                });
    //            }
    //        })
    //        .catch(err => console.error("Edit panel yüklenemedi:", err));
    //}
});