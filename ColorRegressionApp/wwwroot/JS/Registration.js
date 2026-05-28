document.addEventListener("DOMContentLoaded", function () {
    const button = document.getElementById("registerBtn");
    const errorMessage = document.getElementById("errorMessage");

    button.addEventListener("click", async function () {
        const username = document.getElementById("username").value.trim();
        const email = document.getElementById("email").value.trim();
        const password = document.getElementById("password").value.trim();

        errorMessage.style.display = "none";

        if (username === "" || email === "" || password === "") {
            errorMessage.textContent = "Заполните все поля!";
            errorMessage.style.display = "block";
            return;
        }

        try {
            const response = await fetch("/api/Account/Register", {
                method: "POST",
                headers: { "Content-Type": "application/json" },
                body: JSON.stringify({ username, email, password })
            });

            if (!response.ok) {
                const text = await response.text();
                errorMessage.textContent = text || "Ошибка регистрации.";
                errorMessage.style.display = "block";
                return;
            }

            window.location.href = "Auth.html";
        } catch (e) {
            errorMessage.textContent = "Ошибка при регистрации.";
            errorMessage.style.display = "block";
        }
    });
});