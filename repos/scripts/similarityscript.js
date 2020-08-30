customElements.define('include-directive', class extends HTMLElement
{
  async connectedCallback()
  {
    let src = this.getAttribute('src');
    this.innerHTML = await (await fetch(src)).text();
  }

});

function addScrollEventListener()
{
  return;
  alert('1');
  let iframe = document.getElementsByTagName('frame-container')[0];
  alert('2');

if (iframe == null) alert('its null');

  iframe.contentDocument.body.innerHTML = 'a<br>a<br>a<br>a<br>a<br>a<br>a<br>a<br>a<br>a<br>';
  alert('3');

  iframe.contentDocument.addEventListener('scroll', function(event) {
    console.log(event);
  }, false);
  alert('4');

}


function onloadComplete(ms, md)
{
  switch (md)
  {
    case 'index':
      setInterval(waitForMarkdown('index-md'), ms);
      break;

    case 'shell':
      setInterval(waitForMarkdown('shell-md'), ms);
      break;

    case 'repos':
      setInterval(waitForMarkdown('repos-md'), ms);
      break;

    default:
      setInterval(waitForMarkdown('index-md'), ms);
      break;

  }

}

function selectMarkdownDocument(md)
{
  switch (md)
  {
    case 'index':
      getMarkdownDocument('index', 'https://ceresbakalite.github.io/similarity/repos/scripts/SyncIndex.html');
      break;

    case 'shell':
      getMarkdownDocument('shell', 'https://ceresbakalite.github.io/similarity/repos/scripts/SyncShell.html');
      break;

    case 'repos':
      getMarkdownDocument('repos', 'https://ceresbakalite.github.io/similarity/repos/scripts/SyncRepos.html');
      break;

    default:
      getMarkdownDocument('index', 'test01.html');
      break;

  }

}

function getMarkdownDocument(id, target)
{
    document.getElementById('frame-container').setAttribute('src', target);
    document.getElementById(id).blur();
}

function waitForMarkdown(target)
{
  WebComponents.waitFor(() =>  {

    if (refreshMarkdown(target))
    {
      document.getElementById('footer-content').style.display = 'block';
      document.getElementById('site-footer-display').style.display = 'block';
    }

  });

}

function refreshMarkdown(target)
{
  if (document.getElementById(target))
  {
    document.getElementById(target).setAttribute('src', document.getElementById(target).getAttribute('src') + '?' + getRandomInteger(10000,1000000));

  } else if (document.getElementsByTagName('zero-md')[0]) {
    document.getElementsByTagName('zero-md')[0].setAttribute('src', document.getElementsByTagName('zero-md')[0].getAttribute('src') + '?' + getRandomInteger(10000,1000000));

  } else {
    return false;

  }

  return true;
}

function getRandomInteger(min, max)
{
  return Math.floor(Math.random() * (max - min) ) + min;
}

function adjustHeaderDisplay(attribute)
{
  document.getElementById('site-header-display').style.display = attribute;
}
