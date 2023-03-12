function setDialogVisibility(dialogId, isVisible) {
    document.getElementById(dialogId).style.display = isVisible ? "flex" : "none";

    if (!isVisible) {
        window.location.hash = "#";
    }
}

function joinDiscord() {
    window.open("https://discord.com/invite/sJysbdu");
}

let hash = window.location.hash;
switch (hash) {
    case "#privacy-policy":
        setDialogVisibility("privacy-policy", true);
        break;
    case "#contact":
    case "#support":
        setDialogVisibility("support", true);
        break;
}