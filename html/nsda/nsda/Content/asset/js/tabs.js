var tab = document.getElementsByClassName('tab');
var tabt = document.getElementsByClassName('tabt');
var tabc = document.getElementsByClassName('tabc');
var tabcon = document.getElementsByClassName('tabcon');
console.log(tab, tabt, tabc, tabcon)

for (let i = 0; i < tabt.length; i++) {
    tabt[i].index = i;
    tabt[i].onclick = function () {
        for (let j = 0; j < tabt.length; j++) {
            tabcon[j].style.display = "none";
        }
        tabcon[this.index].style.display = "block";
    }
}




