document.addEventListener("DOMContentLoaded", function () {
    const button = document.getElementById("loginBtn");
    const error = document.getElementById("errorMessage");

    if (!error) {
        console.error("errorMessage not found");
        return;
    }

    button.addEventListener("click", async function () {
        const username = document.getElementById("username").value.trim();
        const password = document.getElementById("password").value.trim();

        error.style.display = "none";

        if (username === "" || password === "") {
            error.textContent = "Заполните все поля!";
            error.style.display = "block";
            return;
        }

        try {
            const response = await fetch("/api/Account/Login", {
                method: "POST",
                headers: { "Content-Type": "application/json" },
                body: JSON.stringify({ username, password })
            });

            if (!response.ok) {
                error.textContent = "Неверные имя пользователя или пароль.";
                error.style.display = "block";
                return;
            }

            const data = await response.json();
            console.log("Токен:", data.token);
            window.location.href = "Dashboard.html";
        } catch (e) {
            error.textContent = "Ошибка при входе.";
            error.style.display = "block";
        }
    });
});