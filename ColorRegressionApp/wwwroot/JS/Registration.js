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

        try 
        {
            const response = await fetch("/api/auth/register", 
            {
                method: "POST",
                headers: { "Content-Type": "application/json" },
                body: JSON.stringify({ username, email, password })
            });
            if (!response.ok) 
            {
                error.textContent = "Неверные имя пользователя или пароль.";
                error.style.display = "block";
                return;
            }
            const data = await response.json();

            console.log("Токен:", data.token);

            window.location.href = "Dashboard.html";
        } 
        catch 
        {
            error.textContent = "Ошибка при входе.";
            error.style.display = "block";
        }

        errorMessage.style.display = "none";

        window.location.href = "Dashboard.html";
    });
});