
var tabt = document.getElementsByClassName('tabt');
var tabcon = document.getElementsByClassName('tabcon');

for (let i = 0; i < tabt.length; i++) {
    tabt[i].index = i;
    tabt[i].onclick = function () {
        for (let j = 0; j < tabt.length; j++) {
            tabcon[j].style.display = "none";
            tabt[j].style.border="none"
        }
        tabcon[this.index].style.display = "block";
        tabt[i].style.border = "1px solid #cacaca";
        tabt[i].style.borderBottom = "1px solid white";
        tabt[i].style.borderRadius = "10px 10px 0 0";
    }
}




