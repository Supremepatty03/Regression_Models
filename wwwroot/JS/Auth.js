document.addEventListener("DOMContentLoaded", function () {

    const button = document.getElementById("loginBtn");

    button.addEventListener("click", function () {

        const username = document.getElementById("username").value.trim();
        const password = document.getElementById("password").value.trim();
        const error = document.getElementById("errorMsg");

        if (username === "" || password === "") {
            errorMessage.textContent = "Заполните все поля!";
            errorMessage.style.display = "block";
            return;
        }

        errorMessage.style.display = "none";

        window.location.href = "Dashboard.html";
    });

});