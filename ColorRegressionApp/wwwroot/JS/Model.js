document.addEventListener("DOMContentLoaded", async function () {
    const params = new URLSearchParams(window.location.search);
    const modelId = params.get("modelId");

    const title = document.getElementById("modelTitle");
    const formulaL = document.getElementById("formulaL");
    const formulaA = document.getElementById("formulaA");
    const formulaB = document.getElementById("formulaB");
    const coeffBody = document.getElementById("coefficientsBody");
    const comparisonBody = document.getElementById("comparisonBody");
    const predictionBtn = document.getElementById("goPredictionBtn");

    async function loadModel(url) {
        const response = await fetch(url);

        if (!response.ok) {
            throw new Error(await response.text());
        }

        return await response.json();
    }

    try {
        const data = await loadModel(
            modelId ? `/api/Model/${encodeURIComponent(modelId)}` : "/api/Model/Latest"
        );

        title.textContent = `${data.modelName} — результаты построения модели`;

        formulaL.textContent = data.formulaL || "";
        formulaA.textContent = data.formulaA || "";
        formulaB.textContent = data.formulaB || "";

        coeffBody.innerHTML = "";
        (data.coefficients || []).forEach(row => {
            const tr = document.createElement("tr");
            tr.innerHTML = `
                <td>${row.term}</td>
                <td>${Number(row.l).toFixed(4)}</td>
                <td>${Number(row.a).toFixed(4)}</td>
                <td>${Number(row.b).toFixed(4)}</td>
            `;
            coeffBody.appendChild(tr);
        });

        const realL = data.realValuesL || [];
        const predL = data.predictedValuesL || [];
        const labels = realL.map((_, i) => String(i + 1));

        comparisonBody.innerHTML = "";
        for (let i = 0; i < realL.length; i++) {
            const real = Number(realL[i]);
            const pred = Number(predL[i]);
            const error = Math.abs(real - pred);

            const tr = document.createElement("tr");
            tr.innerHTML = `
                <td>${i + 1}</td>
                <td>${real.toFixed(3)}</td>
                <td>${pred.toFixed(3)}</td>
                <td>${error.toFixed(3)}</td>
            `;
            comparisonBody.appendChild(tr);
        }

        const ctx = document.getElementById("lChart").getContext("2d");

        new Chart(ctx, {
            type: "line",
            data: {
                labels: labels,
                datasets: [
                    {
                        label: "L (реальное)",
                        data: realL,
                        borderWidth: 2
                    },
                    {
                        label: "L (модель)",
                        data: predL,
                        borderWidth: 2
                    }
                ]
            },
            options: {
                responsive: true,
                maintainAspectRatio: false,
                plugins: {
                    legend: {
                        display: true
                    }
                },
                scales: {
                    x: {
                        title: {
                            display: true,
                            text: "Номер точки"
                        }
                    },
                    y: {
                        title: {
                            display: true,
                            text: "Значение L"
                        }
                    }
                }
            }
        });

        predictionBtn.onclick = () => {
            window.location.href = `prediction.html?modelId=${data.modelId}`;
        };
    } catch (e) {
        console.error(e);
        alert("Не удалось загрузить модель. Сначала постройте её на странице ввода данных.");
        window.location.href = "DataInput.html";
    }
});