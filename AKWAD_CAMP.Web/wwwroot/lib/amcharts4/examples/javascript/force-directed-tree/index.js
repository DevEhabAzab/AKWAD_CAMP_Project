am4core.useTheme(am4themes_animated);
am4core.addLicense("ch-custom-attribution");
var chart = am4core.create("chartdiv", am4plugins_forceDirected.ForceDirectedTree);
var networkSeries = chart.series.push(new am4plugins_forceDirected.ForceDirectedSeries())

chart.data = [
  {
    name: "Ahmed Mahmoud \n 1000098956",value:2500,
    children: [
     
      {
        name: "Accounts",value:2000,fixed:true,
        children: [
          { name: "Current",value:1500,
			
				children: [
				  {
					name: "Large \n Incoming Wires \n 1547841",
					children: [
					  { name: "TXN154846122", value: 700 },
					  { name: "TXN154846125", value: 700 },
					  { name: "TXN154846123", value: 700 }
					]
				  },
				  { name: "Large \n Cash Deposits \n 6458452", value:1000,
					children: [
						  { name: "TXN1548461787", value: 1000 }
						]		 
				  }
				]
				
		  },
          { name: "Loan",value:1500,
			children: [
				  { name: "Early \n Termination \n 1547841"	,value:1000 }				  
				]
				
		  },
		  
          { name: "Saving", value:900, }
        ]
      }
   
    ]
  },
  {
    name: "Mona Ali \n 1000098956",value:2500,
    children: [
     
      {
        name: "Accounts",value:2000,fixed:true,
        children: [
          { name: "Current",value:1500,
			
				children: [
				  {
					name: "Large \n Incoming Wires \n 1547841",
					children: [
					  { name: "TXN154846122", value: 700 },
					  { name: "TXN154846125", value: 700 },
					  { name: "TXN154846123", value: 700 }
					]
				  },
				  { name: "Large \n Cash Deposits \n 6458452", value:1000,
					children: [
						  { name: "TXN1548461787", value: 1000 }
						]		 
				  }
				]
				
		  },
          { name: "Loan",value:1500,
			children: [
				  { name: "Early \n Termination \n 1547841"	,value:1000 }				  
				]
				
		  },
		  
          { name: "Saving", value:900, }
        ]
      }
   
    ]
  }
];



networkSeries.dataFields.value = "value";
networkSeries.dataFields.name = "name";
networkSeries.dataFields.children = "children";
networkSeries.nodes.template.tooltipText = "{name}";
networkSeries.nodes.template.fillOpacity = 5;

networkSeries.nodes.template.label.text = "{name}"
networkSeries.fontSize = 15;








	