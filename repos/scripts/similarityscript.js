function getMarkdown(ms, target)
{
  switch (target)
  {
    case 'shell':
      refreshMarkdown_Shell();
      break;

    case 'repos':
      refreshMarkdown_Repos();
      break;

    default:
      refreshMarkdown_Shell();
  }

  var interval = setInterval(waitForMarkdown, ms);
}

function refreshMarkdown_Shell()
{
  document.getElementsById("shell")[0].setAttribute("file", "https://ceresbakalite.github.io/similarity/shell/README.md?" + getRandomInteger(10000,1000000));
}

function refreshMarkdown_Repos()
{
  alert('1');
  document.getElementsById("repos")[0].setAttribute("file", "https://ceresbakalite.github.io/similarity/README.md?" + getRandomInteger(10000,1000000));
  alert('2');
  document.getElementsById("logo")[0].setAttribute("src", "https://ceresbakalite.github.io/similarity/images/NAVSimilarityLogoRepos.png");
  alert('3');
}

function waitForMarkdown()
{
  document.getElementsById("site-footer-display")[0].style.display = "block";
}

function getRandomInteger(min, max)
{
  return Math.floor(Math.random() * (max - min) ) + min;
}
