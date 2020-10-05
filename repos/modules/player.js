export { play }

var play = {};
(function() {

    let test = null;

    this.get = function (name)
    {
        return 'hi ' + name + '!';
    }

    this.set = function (name)
    {
        document.body.innerHTML = get(name));
    }

    window.onload = set('sandy');

}).apply(play);

/*
function sayHi(name)
{
    return 'hi ' + name + '!';
}

function sayThat(name)
{
    return 'hi ' + name + '!';
}

export { sayHi, sayThat }
*/
