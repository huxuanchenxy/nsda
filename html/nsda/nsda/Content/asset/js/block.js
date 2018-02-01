function $$(id) {
    return document.getElementById(id);
}
window.onload = function () {
    $$("search").onclick = function () {
        $$("sel").style.display = "block"
    };
    $$("sel").onclick = function () {
        this.style.display = "none"
        $$("txt").value = this.options[this.selectedIndex].text;
    };
};