function markdownpause(ms)
{
  markdowntarget_shell();
  var interval = setInterval(waitformarkdown, ms);
}

function waitformarkdown()
{
  document.getElementById("site-footer-display").style.display = "block";
}


//function markdowntarget_shell()
//{
//  document.getElementById("zero-md").setAttribute("file", "https://ceresbakalite.github.io/similarity/shell/README.md?" + getRandomInteger(10000,1000000));
//}

function markdowntarget_shell()
{
  var zero-md = document.getElementsById("zero-md");
  var attribute = document.createAttribute("file");
  attribute.value = "https://ceresbakalite.github.io/similarity/shell/README.md?" + getRandomInteger(10000,1000000);
  zero-md.setAttributeNode(attribute);

  alert(attribute.value);
}

function getRandomInteger(min, max)
{
  return Math.floor(Math.random() * (max - min) ) + min;
}
