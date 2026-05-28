document.addEventListener("DOMContentLoaded", function () {
    const button = document.getElementById("calcBtn");
    const errorMessage = document.getElementById("errorMessage");

    button.addEventListener("click", async function () {
        const targetL = Number(document.getElementById("L").value.replace(",", "."));
        const targetA = Number(document.getElementById("a").value.replace(",", "."));
        const targetB = Number(document.getElementById("b").value.replace(",", "."));

        errorMessage.style.display = "none";

        if (Number.isNaN(targetL) || Number.isNaN(targetA) || Number.isNaN(targetB)) {
            errorMessage.textContent = "Заполните все поля корректными числами.";
            errorMessage.style.display = "block";
            return;
        }

        try {
            const response = await fetch("/api/Prediction/Calculate", {
                method: "POST",
                headers: {
                    "Content-Type": "application/json"
                },
                body: JSON.stringify({
                    targetL: targetL,
                    targetA: targetA,
                    targetB: targetB
                })
            });

            if (!response.ok) {
                const text = await response.text();
                throw new Error(text || "Ошибка расчёта прогноза");
            }

            const data = await response.json();

            document.getElementById("x1").textContent = data.resultX1.toFixed(3);
            document.getElementById("x2").textContent = data.resultX2.toFixed(3);
            document.getElementById("x3").textContent = data.resultX3.toFixed(3);

            document.getElementById("resultBlock").style.display = "block";
        } catch (e) {
            console.error(e);
            errorMessage.textContent = "Ошибка при расчёте прогноза.";
            errorMessage.style.display = "block";
        }
    });
});