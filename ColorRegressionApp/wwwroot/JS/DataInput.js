document.addEventListener("DOMContentLoaded", function () {
    const button = document.getElementById("buildModelBtn");
    const errorMsg = document.getElementById("errorMsg");

    button.addEventListener("click", async function () {
        const rows = document.querySelectorAll("tbody tr");
        const points = [];

        let isValid = true;
        let firstEmpty = null;

        rows.forEach((row) => {
            const inputs = row.querySelectorAll("input");

            inputs.forEach((input) => {
                input.classList.remove("input-error");

                if (input.value.trim() === "") {
                    isValid = false;
                    input.classList.add("input-error");
                    if (!firstEmpty) firstEmpty = input;
                }
            });

            const values = Array.from(inputs).map((input) =>
                Number(input.value.replace(",", "."))
            );

            if (values.some((v) => Number.isNaN(v))) {
                isValid = false;
                if (!firstEmpty) firstEmpty = inputs[0];
                return;
            }

            points.push({
                x1: values[0],
                x2: values[1],
                x3: values[2],
                l: values[3],
                a: values[4],
                b: values[5]
            });
        });

        if (!isValid) {
            errorMsg.textContent = "Заполните все поля.";
            errorMsg.style.display = "block";
            if (firstEmpty) firstEmpty.focus();
            return;
        }

        try {
            const response = await fetch("/api/Model/Build", {
                method: "POST",
                headers: {
                    "Content-Type": "application/json"
                },
                body: JSON.stringify({
                    name: "Модель 1",
                    points: points
                })
            });

            if (!response.ok) {
                const text = await response.text();
                throw new Error(text || "Ошибка построения модели");
            }

            const data = await response.json();

            errorMsg.style.display = "none";
            window.location.href = `Model.html?modelId=${data.modelId}`;
        } catch (e) {
            console.error(e);
            errorMsg.textContent = "Ошибка при отправке данных.";
            errorMsg.style.display = "block";
        }
    });
});