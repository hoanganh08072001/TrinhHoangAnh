// Set new default font family and font color to mimic Bootstrap's default styling
Chart.defaults.global.defaultFontFamily = '-apple-system,system-ui,BlinkMacSystemFont,"Segoe UI",Roboto,"Helvetica Neue",Arial,sans-serif';
Chart.defaults.global.defaultFontColor = '#292b2c';

// Area Chart Example
var ctx = document.getElementById("myAreaChart");
var c1 = document.getElementById("c1").innerHTML;
var c2 = document.getElementById("c2").innerHTML;
var c3 = document.getElementById("c3").innerHTML;
var c4 = document.getElementById("c4").innerHTML;
var c5 = document.getElementById("c5").innerHTML;
var c6 = document.getElementById("c6").innerHTML;
var c7 = document.getElementById("c7").innerHTML;
var c8 = document.getElementById("c8").innerHTML;
var c9 = document.getElementById("c9").innerHTML;
var c10 = document.getElementById("c10").innerHTML;
var c11 = document.getElementById("c11").innerHTML;
var c12 = document.getElementById("c12").innerHTML;
var array = [c1, c2, c3, c4, c5, c6, c7, c8, c9, c10, c11, c12];
var maxInNumbers = Math.max.apply(Math, array);
var minInNumbers = Math.min.apply(Math, array);
var myLineChart = new Chart(ctx, {
  type: 'line',
  data: {
      labels: [" January", " February", " March", " April", "May", " June", " July", " August", " September", " October", " November", " December"],
    datasets: [{
      label: "Number",
      lineTension: 0.3,
      backgroundColor: "rgba(2,117,216,0.2)",
      borderColor: "rgba(2,117,216,1)",
      pointRadius: 5,
      pointBackgroundColor: "rgba(2,117,216,1)",
      pointBorderColor: "rgba(255,255,255,0.8)",
      pointHoverRadius: 5,
      pointHoverBackgroundColor: "rgba(2,117,216,1)",
      pointHitRadius: 50,
      pointBorderWidth: 2,
        data: [c1, c2, c3, c4, c5, c6, c7, c8, c9, c10, c11, c12],
    }],
  },
  options: {
    scales: {
      xAxes: [{
        time: {
          unit: 'date'
        },
        gridLines: {
          display: false
        },
        ticks: {
          maxTicksLimit: 12
        }
      }],
      yAxes: [{
        ticks: {
          min: minInNumbers,
          max: maxInNumbers,
          maxTicksLimit: 1
        },
        gridLines: {
          color: "rgba(0, 0, 0, .125)",
        }
      }],
    },
    legend: {
      display: false
    }
  }
});
