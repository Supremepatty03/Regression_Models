document.addEventListener("DOMContentLoaded", function () {
    const modelsGrid = document.getElementById("modelsGrid");
    const emptyState = document.getElementById("emptyState");

    function formatDate(value) {
        if (!value) return "-";
        const date = new Date(value);
        return isNaN(date.getTime()) ? "-" : date.toLocaleDateString("ru-RU");
    }

    async function loadModels() {
        try {
            const response = await fetch("/api/Model/List", {
                cache: "no-store"
            });

            if (!response.ok) {
                throw new Error(await response.text());
            }

            const models = await response.json();
            console.log("Models from API:", models);

            modelsGrid.innerHTML = "";

            if (!Array.isArray(models) || models.length === 0) {
                emptyState.style.display = "block";
                emptyState.textContent = "Сохранённых моделей пока нет.";
                return;
            }

            emptyState.style.display = "none";

            models.forEach(model => {
                const id = model.id ?? model.Id;
                const name = model.name ?? model.Name ?? `Модель №${id}`;
                const createdAt = model.createdAt ?? model.CreatedAt;
                const pointCount = model.pointCount ?? model.PointCount ?? 3;

                const card = document.createElement("div");
                card.className = "model-card";
                card.dataset.modelId = id;

                card.innerHTML = `
                    <h3>${name}</h3>
                    <p>ID: ${id}</p>
                    <p>Дата: ${formatDate(createdAt)}</p>
                    <p>Точек: ${pointCount}</p>

                    <div class="card-buttons">
                        <button type="button" class="open-btn">Открыть</button>
                        <button type="button" class="delete-btn delete">Удалить</button>
                    </div>
                `;

                modelsGrid.appendChild(card);
            });
        } catch (error) {
            console.error(error);
            emptyState.style.display = "block";
            emptyState.textContent = "Не удалось загрузить список моделей.";
        }
    }

    modelsGrid.addEventListener("click", async function (event) {
        const openBtn = event.target.closest(".open-btn");
        const deleteBtn = event.target.closest(".delete-btn");

        if (openBtn) {
            const card = openBtn.closest(".model-card");
            const modelId = card?.dataset.modelId;

            if (!modelId) {
                alert("Не удалось определить модель.");
                return;
            }

            window.location.href = `Model.html?modelId=${encodeURIComponent(modelId)}`;
            return;
        }

        if (deleteBtn) {
            const card = deleteBtn.closest(".model-card");
            const modelId = card?.dataset.modelId;

            if (!modelId) {
                alert("Не удалось определить модель.");
                return;
            }

            if (!confirm("Удалить выбранную модель?")) return;

            try {
                const response = await fetch(`/api/Model/${encodeURIComponent(modelId)}`, {
                    method: "DELETE"
                });

                if (!response.ok) {
                    throw new Error(await response.text());
                }

                await loadModels();
            } catch (error) {
                console.error(error);
                alert("Не удалось удалить модель.");
            }
        }
    });

    loadModels();
});