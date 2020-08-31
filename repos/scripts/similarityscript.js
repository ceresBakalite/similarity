
customElements.define('include-directive', class extends HTMLElement
{
  async connectedCallback()
  {
    let src = this.getAttribute('src');
    this.innerHTML = await (await fetch(src)).text();
  }

});

function redirectRequest() 
{
  let target = getURLArgs()[t];
  if (target != null) alert(target);

}

function getUrlArgs()
{
    let args = {};
    var parts = window.location.href.replace(/[?&]+([^=&]+)=([^&]*)/gi, function(m, key, value)
    {
        args[key] = value;
    });

    return args;
}

function addScrollEventListener()
{
  return;

  let iframe = document.getElementsByTagName('frame-container')[0];

  if (iframe == null) alert('its null');

  iframe.contentDocument.body.innerHTML = 'a<br>a<br>a<br>a<br>a<br>a<br>a<br>a<br>a<br>a<br>';

  iframe.contentDocument.addEventListener('scroll', function(event) {
    console.log(event);
  }, false);

}

function isValidSource(md)
{
    if (parent.document.getElementById('primary-container')) return true;

    window.location.href = 'https://ceresbakalite.github.io/similarity/?t=' + md;

    return false;
}


function onloadComplete(ms, md)
{
  if (isValidSource(md))
  {
    switch (md)
    {
      case 'index':
        setTimeout(function(){ waitForMarkdown('index-md'); }, ms);
        break;

      case 'shell':
        setTimeout(function(){ waitForMarkdown('shell-md'); }, ms);
        break;

      case 'repos':
        setTimeout(function(){ waitForMarkdown('repos-md'); }, ms);
        break;

      default:
        setTimeout(function(){ waitForMarkdown('index-md'); }, ms);
        break;

    }

  }

}

function selectMarkdownDocument(md)
{
  switch (md)
  {
    case 'index':
      getMarkdownDocument('index', 'https://ceresbakalite.github.io/similarity/repos/scripts/SyncIndex.html');
      //getMarkdownDocument('index', 'C:\Users\Sandy\Documents\GitHub\similarity\repos\scripts\SyncIndex.html');
      break;

    case 'shell':
      getMarkdownDocument('shell', 'https://ceresbakalite.github.io/similarity/repos/scripts/SyncShell.html');
      break;

    case 'repos':
      getMarkdownDocument('repos', 'https://ceresbakalite.github.io/similarity/repos/scripts/SyncRepos.html');
      break;

    default:
      getMarkdownDocument('index', 'https://ceresbakalite.github.io/similarity/repos/scripts/SyncIndex.html');
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
    document.getElementById('site-footer-display').style.display = 'block';
    document.getElementById('footer-content').style.display = 'block';

    refreshMarkdown(target);
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
