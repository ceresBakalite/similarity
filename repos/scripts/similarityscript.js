function getMarkdown(ms, target)
{
  switch (target)
  {
    case 'index':
      refreshMarkdown_Index();
      break;

    case 'shell':
      refreshMarkdown_Shell();
      break;

    case 'repos':
      refreshMarkdown_Repos();
      break;

    default:
      refreshMarkdown_Shell();
  }

  setInterval(waitForMarkdown, ms);
}

function refreshMarkdown_Index()
{
  WebComponents.waitFor(() =>
  {
      document.getElementsByTagName('zero-md')[0].setAttribute('file', 'https://ceresbakalite.github.io/similarity/repos/scripts/index.md?' + getRandomInteger(10000,1000000));
  });

}

function refreshMarkdown_Shell()
{
  WebComponents.waitFor(() =>
  {
      document.getElementsByTagName('zero-md')[0].setAttribute('file', 'https://ceresbakalite.github.io/similarity/shell/README.md?' + getRandomInteger(10000,1000000));
  });

}

function refreshMarkdown_Repos()
{
  WebComponents.waitFor(() =>
  {
      document.getElementsByTagName('zero-md')[0].setAttribute('file', 'https://ceresbakalite.github.io/similarity/README.md?' + getRandomInteger(10000,1000000));
  });

}

function waitForMarkdown()
{
  WebComponents.waitFor(() =>
  {
      getURL();
      document.getElementById('site-footer-display').style.display = 'block';
  });

}

function getRandomInteger(min, max)
{
  return Math.floor(Math.random() * (max - min) ) + min;
}

var getURL = function (url, success, error) {
    if (!window.XMLHttpRequest) return;
    var request = new XMLHttpRequest();
    request.onreadystatechange = function () {
        if (request.readyState === 4) {
            if (request.status !== 200) {
                if (error && typeof error === 'function') {
                    error(request.responseText, request);
                }
                return;
            }
            if (success && typeof success === 'function') {
                success(request.responseText, request);
            }
        }
    };
    request.open('GET', url);
    request.send();
};

getURL(
    'https://ceresbakalite.github.io/similarity/repos/scripts/CodeIncludeFooter.html',
    function (data) {
        var el = document.createElement(el);
        el.innerHTML = data;
        var fetch = el.querySelector('#new-footer');
        var embed = document.querySelector('#footer');
        alert('here now 2');
        if (!embed) return;
        alert('here now 3');
        if (!fetch) return;
        //if (!fetch || !embed) return;
        alert('here now 4');
        embed.innerHTML = fetch.innerHTML;

    }
);
