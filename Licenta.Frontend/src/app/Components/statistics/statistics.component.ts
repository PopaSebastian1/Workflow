import { Component, OnInit, Input, OnChanges, ViewChildren, ElementRef, QueryList, AfterViewInit, SimpleChanges, Output, EventEmitter } from '@angular/core';
import interact from 'interactjs';
import { 
  Chart, 
  PieController, 
  DoughnutController, 
  BarController, 
  LineController, 
  RadarController, 
  PolarAreaController, 
  ScatterController, 
  CategoryScale, 
  LinearScale, 
  RadialLinearScale, 
  PointElement, 
  LineElement, 
  BarElement, 
  ArcElement, 
  Tooltip, 
  Legend, 
  ChartConfiguration, 
  Title, 
  ChartType 
} from 'chart.js';

Chart.register(
  PieController, 
  DoughnutController, 
  BarController, 
  LineController, 
  RadarController, 
  PolarAreaController, 
  ScatterController, 
  CategoryScale, 
  LinearScale, 
  RadialLinearScale, 
  PointElement, 
  LineElement, 
  BarElement, 
  ArcElement, 
  Tooltip,
  Legend, 
  Title
);

@Component({
  selector: 'app-statistics',
  templateUrl: './statistics.component.html',
  styleUrls: ['./statistics.component.scss']
})
export class StatisticsComponent implements OnInit, OnChanges, AfterViewInit {
  @Input() titles: string[] = [];
  @Input() data: number[][] = [];
  @Input() labels: string[][] = [];
  @Output() chartRemoved = new EventEmitter<number>();
  @ViewChildren('chartCanvas') chartCanvases: QueryList<ElementRef<HTMLCanvasElement>> | undefined;
  chartTypes: ChartType[] = []; // Store the type of each chart separately
  private charts: Chart[] = [];

  ngOnInit(): void {
    this.chartTypes = Array(this.data.length).fill('pie'); // Initialize all as 'pie'
    console.log('ngOnInit - data:', this.data);
    console.log('ngOnInit - labels:', this.labels);
  }

  ngOnChanges(changes: SimpleChanges): void {
    console.log('ngOnChanges - data:', this.data);
    console.log('ngOnChanges - labels:', this.labels);
    this.createCharts();
  }

  
ngAfterViewInit(): void {
  this.createCharts();

}

  changeChartType(index: number): void {
    if (this.charts[index]) {
      this.charts[index].destroy();
      this.createChart(index);
    }
  }

  private createCharts(): void {
    if (this.charts) {
      this.charts.forEach(chart => chart.destroy());
    }
    this.charts = [];

    this.chartCanvases?.toArray().forEach((canvasRef, i) => {
      this.createChart(i);
    });
  }

  private createChart(index: number): void {
    const canvas = this.chartCanvases?.toArray()[index].nativeElement;
    if(!canvas)
      {
        return;
      }
    const ctx = canvas.getContext('2d');
    
    if (ctx) {
      // Set dimensions based on chart type
      switch (this.chartTypes[index]) {
        case 'pie':
          canvas.width = 400;
          canvas.height = 400;
          break;
        case 'doughnut':
          canvas.width = 450;
          canvas.height = 450;
          break;
        case 'polarArea':
          canvas.width = 500;
          canvas.height = 500;
          break;
        case 'bar':
          canvas.width = 550;
          canvas.height = 550;
          break;
        case 'line':
          canvas.width = 600;
          canvas.height = 600;
          break;
        case 'radar':
          canvas.width = 650;
          canvas.height = 650;
          break;
        case 'scatter':
          canvas.width = 700;
          canvas.height = 700;
          break;
        default:
          canvas.width = 400;
          canvas.height = 400;
      }

      const colors = ['#FF6384', '#36A2EB', '#FFCE56', '#4BC0C0', '#9966FF','#E91E63', '#00BCD4', '#4CAF50', '#FFEB3B', '#3F51B5', '#9C27B0', '#F44336', '#03A9F4', '#8BC34A', '#FF9800'];
      const backgroundColor = this.data[index].map((_, i) => colors[i % colors.length]);

      const config: ChartConfiguration = {
        type: this.chartTypes[index],
        data: {
          labels: this.labels[index],
          datasets: [{
            data: this.data[index],
            backgroundColor: backgroundColor,
          }]
        },
        options: {
          responsive: true,
          plugins: {
            tooltip: {
              enabled: true
            },
            legend: {
              display: this.chartTypes[index] === 'pie' || this.chartTypes[index] === 'doughnut' || this.chartTypes[index] === 'polarArea',
              position: 'top',
            },
            title: {
              display: true,
              text: this.titles[index]
            }
          },
          scales: this.chartTypes[index] !== 'pie' && this.chartTypes[index] !== 'doughnut' && this.chartTypes[index] !== 'polarArea' ? {
            x: {
              display: true,
              title: {
                display: true,
                text: 'X Axis'
              }
            },
            y: {
              display: true,
              title: {
                display: true,
                text: 'Y Axis'
              }
            }
          } : {}
        }
      };

      this.charts[index] = new Chart(ctx, config);
    }
  }
  removeChart(index: number): void {
    if (this.charts[index]) {
      this.charts[index].destroy();
      this.chartRemoved.emit(index);
    }
  }
}
