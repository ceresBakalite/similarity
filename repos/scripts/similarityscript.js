function getMarkdown(ms, target, url)
{
  switch (target)
  {
    case 'shell':
      refreshMarkdown_Shell();
      break;

    case 'repos':
      resetMarkdown_Logo(url);
      refreshMarkdown_Repos();
      resetMarkdown_Repos();
      break;

    default:
      refreshMarkdown_Shell();
  }

  var interval = setInterval(waitForMarkdown, ms);
}

function refreshMarkdown_Shell()
{
  alert('refreshMarkdown_Shell');
  document.getElementsByTagName("zero-md")[0].setAttribute("file", "https://ceresbakalite.github.io/similarity/shell/README.md?" + getRandomInteger(10000,1000000));
  alert('refreshMarkdown_Shell complete');
}

function refreshMarkdown_Repos()
{
  alert('refreshMarkdown_Repos');
  document.getElementsByTagName("zero-md")[0].setAttribute("file", "https://ceresbakalite.github.io/similarity/README.md?" + getRandomInteger(10000,1000000));
  alert('refreshMarkdown_Repos complete');
}

function resetMarkdown_Repos()
{
  alert('resetMarkdown_Repos');
  document.getElementById("logo-default").setAttribute("src", "https://ceresbakalite.github.io/similarity/images/NAVSimilarityLogoRepos.png");
  alert('resetMarkdown_Repos complete');
}

function resetMarkdown_Logo(url)
{
  var src = (url == null) ? "https://ceresbakalite.github.io/similarity/images/NAVSimilarityLogoRepos.png" : url;
  //document.getElementById("logo-default").setAttribute("src", "https://ceresbakalite.github.io/similarity/images/NAVSimilarityLogoRepos.png");
}

function waitForMarkdown()
{
  alert('waitForMarkdown');
  document.getElementById("site-footer-display").style.display = "block";
}

function getRandomInteger(min, max)
{
  return Math.floor(Math.random() * (max - min) ) + min;
}

Object.prototype.isEmpty = function()
{
    for (var key in this)
    {
        if(this.hasOwnProperty(key)) return false;
    }

    return true;
}
