using System;
using System.Collections.Generic;

public class Individual{
    
    private Random r = new Random();
    private List<float> genome;
    private float fitness;
    public Boolean valid; // Is solution still valid?


    // Used for creating random solutions. Needed?
    public Individual(){
	genome = new List<float>();
        fitness = 0f;
        genome.Add((float)r.NextDouble()*100);
        System.Threading.Thread.Sleep(10);
        genome.Add((float)r.NextDouble()*10);
        System.Threading.Thread.Sleep(10);
        genome.Add((float)r.NextDouble()*10000);
        System.Threading.Thread.Sleep(10);
        genome.Add((float)r.NextDouble()*100);
        System.Threading.Thread.Sleep(10);
        genome.Add((float)r.NextDouble()*10);
        System.Threading.Thread.Sleep(10);
        genome.Add((float)r.NextDouble()*10000);
        System.Threading.Thread.Sleep(10);
        /*
        genome.Add(50);
        genome.Add(1);
        genome.Add(3000);
        genome.Add(10);
        genome.Add(0);
        genome.Add(300);
        */
        /*
        genome.Add(101.1342f);
        genome.Add(0f);
        genome.Add(2955.052f);
        genome.Add(126.2401f);
        genome.Add(0f);
        genome.Add(154.357f);
        */
        /*
        genome.Add(0f);
        genome.Add(0f);
        genome.Add(2853f);
        genome.Add(104.69f);
        genome.Add(0f);
        genome.Add(138.357f);
        */
        /*
        genome.Add(40.53807f);
        genome.Add(78.27749f);
        genome.Add(2906.992f);
        genome.Add(163.64f);
        genome.Add(81.065f);
        genome.Add(208.357f);
        */
        /*
        genome.Add(165.0367f);
        genome.Add(171.6158f);
        genome.Add(2844.305f);
        genome.Add(0.3249f);
        genome.Add(201.065f);
        genome.Add(150.0f);
        */
    }

    // Used for creating copies.
    public Individual(Individual i){
	genome = new List<float>(i.genome);
        fitness = 0f;
    }

    public Individual(List<float> i){
	genome = new List<float>(i);
        fitness = 0f;
    }

    public List<float> getGenome(){
	return this.genome;
    }

    public void setFitness(float f){
        this.fitness = f;
    }

    public float getFitness(){
	return this.fitness;
    }

    public void mutar(int i, int maxChange){
	float c;
	int s;
	c=(float)(r.NextDouble()*10000) % maxChange;
	s=r.Next(2);
	if(s==1)
    	    genome[i] += c;
	else
	    genome[i] -= c;

        if (genome[i] < 0)
            genome[i] = 0;
    }

    public String toString(){
	String s = "genome: ";
	foreach(float i in genome){
	    s += (i + " ");
	}
        s+="fitness: " + fitness;
	return s;
    }
}
