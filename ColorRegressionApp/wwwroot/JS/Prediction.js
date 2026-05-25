function calculate() {

    // получение значений (пока не используется)
    const L = document.getElementById("L").value;
    const a = document.getElementById("a").value;
    const b = document.getElementById("b").value;

    // заглушка
    document.getElementById("x1").innerText = (Math.random() * 2 + 1).toFixed(2);
    document.getElementById("x2").innerText = (Math.random() * 40 + 40).toFixed(1);
    document.getElementById("x3").innerText = (Math.random() * 30 + 10).toFixed(1);

    document.getElementById("resultBlock").style.display = "block";
}

document.addEventListener("DOMContentLoaded", function () {
    const button = document.getElementById("calcBtn");
    const errorMsg = document.getElementById("errorMessage");

    function calculate() {
        document.getElementById("x1").textContent = (Math.random() * 2 + 1).toFixed(2);
        document.getElementById("x2").textContent = (Math.random() * 40 + 40).toFixed(1);
        document.getElementById("x3").textContent = (Math.random() * 30 + 10).toFixed(1);

        document.getElementById("resultBlock").style.display = "block";
    }

    const inputs = document.querySelectorAll(".input-block input");

    button.addEventListener("click", function () {
        let isValid = true;

        inputs.forEach(input => {
            input.classList.remove("input-error");

            if (input.value.trim() === "") {
                input.classList.add("input-error");
                isValid = false;
            }
        });

        if (!isValid) {
            errorMsg.textContent = "Заполните все поля!";
            errorMsg.style.display = "block";
            document.getElementById("resultBlock").style.display = "none";
            return;
        }

        errorMsg.style.display = "none";
        calculate();
    });

    inputs.forEach(input => {
        input.addEventListener("input", function () {
            if (input.value.trim() !== "") {
                input.classList.remove("input-error");
            }
        });
    });
});
