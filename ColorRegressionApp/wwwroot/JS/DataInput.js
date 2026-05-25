document.addEventListener("DOMContentLoaded", function () {
    const button = document.getElementById("buildModelBtn");
    const errorMsg = document.getElementById("errorMsg");

    button.addEventListener("click", function () {
        const inputs = document.querySelectorAll("tbody input");
        let isValid = true;
        let firstEmpty = null;

        inputs.forEach(input => {
            const inputs = row.querySelectorAll("input");

            points.push(
            {
                x1: parseFloat(inputs[0].value),
                x2: parseFloat(inputs[1].value),
                x3: parseFloat(inputs[2].value),
                l: parseFloat(inputs[3].value),
                a: parseFloat(inputs[4].value),
                b: parseFloat(inputs[5].value)
            });
        });

        if (!isValid) {
            errorMsg.textContent = "Заполните все поля во всех трех строках.";
            errorMsg.style.display = "block";

            if (firstEmpty) {
                firstEmpty.focus();
            }
            return;
        }

        const response = await fetch("/api/model/build",
        {
            method: "POST",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify({ experimentPoints: points })
        });
        
        const data = await response.json();

        localStorage.setItem("modelData", JSON.stringify(data));

        errorMsg.style.display = "none";
        window.location.href = "Model.html";
    });
});