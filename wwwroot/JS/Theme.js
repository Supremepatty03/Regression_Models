document.addEventListener("DOMContentLoaded", function () {
    const button = document.createElement("button");
    button.id = "themeToggleBtn";
    button.textContent = "🌙";

    button.style.position = "fixed";
    button.style.top = "20px";
    button.style.right = "20px";
    button.style.padding = "10px";
    button.style.fontSize = "18px";
    button.style.cursor = "pointer";
    button.style.border = "none";
    button.style.zIndex = "1000";
    button.style.backgroundColor = "#111111";
    button.style.length = "30px";
    button.style.width = "60px";


    document.body.appendChild(button);

    const savedTheme = localStorage.getItem("theme");
    if (savedTheme === "dark") {
        document.body.classList.add("dark-theme");
        button.textContent = "☀️";
    }

    button.addEventListener("click", function () {
        document.body.classList.toggle("dark-theme");
        if (document.body.classList.contains("dark-theme")) {
            button.textContent = "☀️";
            localStorage.setItem("theme", "dark");
        } else {
            button.textContent = "🌙";
            localStorage.setItem("theme", "light");
        }
    });
});