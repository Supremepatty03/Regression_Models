document.addEventListener("DOMContentLoaded", function () {
    const button = document.getElementById("buildModelBtn");
    const errorMsg = document.getElementById("errorMsg");

    button.addEventListener("click", function () {
        const inputs = document.querySelectorAll("tbody input");
        let isValid = true;
        let firstEmpty = null;

        inputs.forEach(input => {
            input.classList.remove("input-error");

            if (input.value.trim() === "") {
                isValid = false;
                input.classList.add("input-error");

                if (!firstEmpty) {
                    firstEmpty = input;
                }
            }
        });

        if (!isValid) {
            errorMsg.textContent = "Заполните все поля во всех трех строках.";
            errorMsg.style.display = "block";

            if (firstEmpty) {
                firstEmpty.focus();
            }
            return;
        }

        errorMsg.style.display = "none";
        window.location.href = "Model.html";
    });
});