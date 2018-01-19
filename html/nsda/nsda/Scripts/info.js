var add = document.getElementById("addedu");
var addeducation = document.getElementById("addeducation");
var close = document.getElementById("close");
var confirmadd = document.getElementById("confirmadd");
console.log(close)
add.onclick = function () {
    addeducation.style.display = "block";
}
close.onclick = function () {
    addeducation.style.display = "none";
}
confirmadd.onclick = function () {
    addeducation.style.display = "none";
}