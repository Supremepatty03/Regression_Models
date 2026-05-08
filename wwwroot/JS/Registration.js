document.addEventListener("DOMContentLoaded", function () {
    const button = document.getElementById("registerBtn");
    button.addEventListener("click", function () {
        const username = document.getElementById("username").value.trim();
        const email = document.getElementById("email").value.trim();
        const password = document.getElementById("password").value.trim();
        const errorMessage = document.getElementById("errorMessage");

        if (username === "" || email === "" || password === "") {
            errorMessage.textContent = "Заполните все поля!";
            errorMessage.style.display = "block";
            return;
        }
        errorMessage.style.display = "none";

        window.location.href = "Dashboard.html";
    });
});