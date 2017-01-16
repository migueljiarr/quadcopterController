/* Algoritmo genetico basico:
Generar una POBLACION aleatoria de N individuos 
Hasta haber realizado G iteraciones 
    Hacer NUEVAPOBLACION = {}
    Hasta completar NUEVAPOBLACION (generar N nuevos individuos)
    	Aplicar operador SELECCIÓN para extraer 1 individuo de la población
    	Aplicar al resultado del paso anterior el operador MUTACIÓN con probabilidad Pm
    	Añadir el resultado del paso anterior a NUEVAPOBLACION 
    Copiar NUEVAPOBLACION en POBLACION
Devolver como solución el individuo con mayor calidad de la POBLACION
*/

using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using UnityEngine;
using Random=System.Random;
 
public class GA:MonoBehaviour{
    public int numGenerations;
    public int numIndividuals;
    public float probMut;
    public int simulationTime;
    public GameObject qC;
    private List<Individual> population;
    private List<Individual> newPop;
    private List<float> probs;
    private List<float> probsSum;
    private Individual basicSolution;

    private float improved;

    private float fitTmp, fitness;
    private Random r;

    public void Start(){
        initializeEvolution();
    }

    public void initializeEvolution(){
        Debug.Log ("Starting simulation.");
	basicSolution = new Individual();
	r = new Random();
	
	population = new List<Individual>(numIndividuals);
	for(int i=0;i<numIndividuals;i++){
	    population.Add(new Individual(basicSolution));
	}
	probs = new List<float>(numIndividuals);
	probsSum = new List<float>(numIndividuals);

	printPop();
        StartCoroutine(evolution());
    }

    void startSimulation (){
        qC.SetActive(true);
    }

    void stopSimulation (){
        qC.SetActive(false);
    }

    IEnumerator evolution() {
	int generation = 0, i;
	Individual iTmp = null;
	Individual ind = null;
	while (generation <= this.numGenerations) {
	    Debug.Log("Generation = " + generation);
	    evaluate(probs,probsSum);
	    newPop = new List<Individual>(numIndividuals);
            startSimulation();
	    for(i=0;i<numIndividuals;i++){
		ind = this.select(population,probsSum);
		iTmp = new Individual(ind);
		//Debug.Log("Individual ind: " + ind.toString());
		// si <= probMut => operador de variacion: intercambiar
		if (r.NextDouble() <= probMut)
		    iTmp.mutar();
		//Debug.Log("Individual iTmp: " + iTmp.toString());
                yield return new WaitForSeconds(simulationTime);
		fitTmp = iTmp.getFitness();
		fitness= ind.getFitness();
		//Debug.Log("fitTmp: " + fitTmp);
		//Debug.Log("fitness: " + fitness);
		if (fitTmp > fitness){
		    //population[population.IndexOf(ind)] = new Individual(iTmp);
		    newPop.Add(iTmp);
		    //Debug.Log("improved. fitTmp: " + fitTmp + ", fitness: " + fitness);
		}
		else{
		    newPop.Add(ind);
		    //Debug.Log("NOT improved. fitTmp: " + fitTmp + ", fitness: " + fitness);
		}
	    }
            stopSimulation();
	    population = newPop;
	    mostrarMejorYMedia(generation);
	    //printPop();
	    generation++;
	}
	mostrarResultado();
        Debug.Log ("Ending simulation.");
    }

    public Individual select(List<Individual> pop, List<float> pSum){
	float s = (float)r.NextDouble();
	//Debug.Log("s: " + s);
	int i=0;
	while(i<numIndividuals-1){
	    //Debug.Log("pSum["+i+"]: " + pSum[i]);
	    if(pSum[i] < s){
		i++;
	    }
	    else{
		break;
	    }
	}
	//Debug.Log("We choose i: " + i + ", pSum[i]: " + pSum[i]);
	return pop[i];
    }

    public void evaluate(List<float> p, List<float> sum){
	float total=0,f;
	List<float> fit = new List<float>(numIndividuals);
	
	//Calculate fitness of each Individual.
	foreach(Individual ind in population){
	    f = (float)ind.getFitness();
	    total+=(float)f;
	    fit.Add(f);
	}

	//Calculate probability for each Individual as it's quality divided by the total quality of the population.
	for(int i=0;i<numIndividuals;i++){
	    p.Insert(i,(float)fit[i]/total);
	    //Debug.Log("aux: " + aux);
	    //Debug.Log("i: " + i + ", ind: " + population[i].toString() + ", p(i): " + p[i]);
            //Debug.Log(String.Format("  {0:F20}", aux));
            //Debug.Log(String.Format("  {0:F20}", p[i]));
	}
	//Debug.Log("total: " + total);
	
	total=0;
	//Calculate acumulated probability for selection.
	for(int i=0;i<numIndividuals;i++){
	    total += p[i];
	    //Debug.Log("i: " + i + ", a(i): " + total);
	    sum.Insert(i,total);
	}
    }

    public void printPop(){
	int n=0; 
	foreach (Individual i in population){
	    n++;
	    Debug.Log ("Individual number: " + n + " " + i.toString());
	}
    }
    
    public void mostrarMejorYMedia(int g){
	float mayor = 0;
	float fit = 0;
	float total = 0;
	foreach (Individual i in population) {
	    fit = i.getFitness();
	    total += fit;
	    if (fit > mayor)
		mayor = fit;
	}
	if(g==0){
	    this.improved=(total/population.Count());
	}
	if(g==199){
	    this.improved=(total/population.Count())-this.improved;
	    Debug.Log("IMPROVED: " + this.improved);
	}
	Debug.Log("Best solution = " + mayor*1000);
	Debug.Log("Population mean = " + (total / population.Count()));
    }

    public void mostrarResultado(){
	float mayor = 0;
	float fit = 0;
	float total = 0;
	int numInd = 0;
	Individual mejor = null;
	Debug.Log("Final results:");
	Debug.Log("\n===================\n");
	Debug.Log("Final Pop:");
	foreach (Individual i in population) {
		fit = i.getFitness();
		Debug.Log("Individual " + ++numInd + ": " + i.toString() + ", fitness: " + fit);
		total += fit;
		if (fit > mayor) {
			mayor = fit;
			mejor = i;
		}
	}
	Debug.Log("Number of generations: " + this.numGenerations);
	Debug.Log("Best global solution = " + mejor.toString() + ", fitness: " + mejor.getFitness());
	Debug.Log("Last generation mean = " + (total / population.Count()));
    }

    public bool getOscillation(Vector3 prevPos, Vector3 curPos, Vector3 targetPos){
        if(prevPos.magnitude < targetPos.magnitude && curPos.magnitude > targetPos.magnitude)
            return true;
        else if(prevPos.magnitude > targetPos.magnitude && curPos.magnitude < targetPos.magnitude)
            return true;
        else return false;
    }
}
