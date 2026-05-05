document.getElementById("calcBtn").addEventListener("click", function () {
    document.getElementById("x1").textContent = (Math.random() * 2 + 1).toFixed(2);
    document.getElementById("x2").textContent = (Math.random() * 40 + 40).toFixed(1);
    document.getElementById("x3").textContent = (Math.random() * 30 + 10).toFixed(1);

    document.getElementById("resultBlock").style.display = "block";
});