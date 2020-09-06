var ceres = {};
(function(slideview)
{
    let index = 1;
    let css = true; // default - use slideviewer css
    let ptr = true; // default - display slideviewer pointers
    let sur = true; // default - display slideviewer surtitles
    let sub = true; // default - display slideviewer subtitles

    window.customElements.define('ceres-slideview', class extends HTMLElement
    {
        async connectedCallback()
        {
            let src = this.getAttribute('src');
            this.innerHTML = await (await fetch(src)).text();
        }

    });

    slideview.openImageTab = function(el) { window.open(el.getAttribute('src'), 'image'); };

    slideview.getSlide = function(targetIndex) { displaySlide(index += targetIndex); };

    slideview.setSlide = function(targetIndex) { displaySlide(index = targetIndex); };

    slideview.slideViewer = function()
    {
        //getSlideViewercss();
        getSlideViewer();
        displaySlide();
    }

    slideview.CSSDone = function(str)
    {
        document.getElementById('ceres-slideview-image-container').style.display = 'block';
        getSlideViewer();
        displaySlide();
    }

    function getSlideViewercss()
    {
        const link = document.createElement('link');

        link.rel = 'stylesheet';
        link.type = 'text/css';
        link.href = 'https://ceresbakalite.github.io/similarity/stylesheets/similaritysheetslide.css';
        link.as = 'style';

        link.onload = function () {
          slideview.CSSDone('onload listener');
        }

        if (link.addEventListener) {
          link.addEventListener('load', function() {
            slideview.CSSDone("DOM's load event");
          }, false);
        }

        link.onreadystatechange = function() {
          var state = link.readyState;
          if (state === 'loaded' || state === 'complete') {
            link.onreadystatechange = null;
            slideview.CSSDone("onreadystatechange");
          }
        };

        var cssnum = document.styleSheets.length;
        var ti = setInterval(function() {
          if (document.styleSheets.length > cssnum) {
            // needs more work when you load a bunch of CSS files quickly
            // e.g. loop from cssnum to the new length, looking
            // for the document.styleSheets[n].href === url
            // ...

            // FF changes the length prematurely :()
            slideview.CSSDone('listening to styleSheets.length change');
            clearInterval(ti);

          }
        }, 10);

        document.head.appendChild(link);
    };

    function displaySlide(targetIndex)
    {
        const slides = document.querySelectorAll(".slideview");
        const pointers = document.querySelectorAll(".ptr");

        index = (targetIndex < 1) ? slides.length : (targetIndex > slides.length) ? 1 : index;

        slides.forEach(slide => { slide.style.display = 'none';	});
        slides[index-1].style.display = 'block';

        if (pointers != null)
        {
            pointers.forEach(pointer => { pointer.className = pointer.className.replace(' active', '');	});
            pointers[index-1].className += ' active';
        }

    };

    function getSlideViewer()
    {
        let progenitor = (document.getElementById("ceres-slideview")) ? document.getElementById("ceres-slideview") : document.getElementsByTagName('ceres-slideview')[0];
        let ar = getSlideViewerAttributes();

        createSlideViewContainer();

        function getSlideViewerAttributes()
        {
            ptr = (progenitor.getAttribute('ptr')) ? progenitor.getAttribute('ptr') : ptr;
            sub = (progenitor.getAttribute('sub')) ? progenitor.getAttribute('sub') : sub;
            sur = (progenitor.getAttribute('sur')) ? progenitor.getAttribute('sur') : sur;
            css = (progenitor.getAttribute('css')) ? progenitor.getAttribute('css') : css;

            return imageListToArray(progenitor.innerHTML);

            function imageListToArray(str)
            {
                return str.replace(/((<([^>]+)>)| )/gi, '').trim().replace(/\r\n|\r|\n/gi, ';').split(';');
            }

        }

        // create ceres-slideview-image-container and class
        function createSlideViewContainer()
        {
            //let parent = document.createElement('div');

            //parent.createAttribute('id');
            //parent.createAttribute('class');
            //parent.setAttribute('id', 'ceres-slideview-image-container');
            //parent.setAttribute('class', 'slideview-image-container');

            //progenitor.appendChild(parent);

            ar.forEach(item =>
            {
                let urlArray = item.split(',');
                //let svname = 'slideview' + item;
                //let surName = 'slideview-sur' + item;
                //let subName = 'slideview-sub' + item;
                //let imgName = 'slideview-img' + item;

                alert('item: ' + item);
                //alert('svname: ' + svname);
                //alert('surName: ' + surName + ' - ' + getSurtitle());
                //alert('subName: ' + subName + ' - ' + getSubtitle());
                //alert('imgName: ' + imgName);
                //alert('src: ' + urlArray[0]);

//                setDivElement(svname, 'slideview fade', parent, null);

//                child = document.getElementById(svname);

//                setDivElement(surName, 'surtitle', child, getSurtitle());
//                setImgElement(imgName, 'ceres.openImageTab(this);', child);
//                setDivElement(subName, 'subtitle', child, getSubtitle());
            });

            function getSurtitle()
            {
                return (sur) ? item + ' / ' + ar.Length : null;
            }

            function getSubtitle()
            {
                if (!sub) return null;
                return (urlArray[1] != null) ? urlArray[1] : null;
            }

            // create slideview+n and class append child
            function setDivElement(str, strc, obj, strhtml)
            {
                let el = document.createElement('div');

                el.createAttribute('id');
                el.createAttribute('class');
                el.setAttribute('id', str);
                el.setAttribute('class', strc);

                if (strhtml != null) el.innerHTML = strhtml;

                obj.appendChild(el);

            }

            // create slideview-img+n and onclick event append child
            function setImgElement(str, strc, obj)
            {
                let el = document.createElement('img');

                el.createAttribute('id');
                el.createAttribute('onclick');
                el.createAttribute('src');
                el.setAttribute('id', str);
                el.setAttribute('onclick', strc);
                el.setAttribute('src', urlArray[0]);

                obj.appendChild(el);
            }

        }


        // create br append child
        function createLineBreakContainer()
        {
        }

        // create ceres-slideview-pointer-container and class
        function createCeresSlideviewPointerContainer()
        {
        }

        // create slideview-ptr+n and class and onclick event append child
        function createSlideviewPointerContainer()
        {
        }

    }

})(ceres);
