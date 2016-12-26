using System;
using System.Collections.Generic;

public class Individual{
    
    private Random r;
    private List<int> genome;

    // Used for initialitation of basicSolution. Needed?
    public Individual(int i){
	r = new Random();
	genome = new List<int>();
	genome.Add(i);
    }

    // Used for creating random solutions. Needed?
    public Individual(){
	r = new Random();
	genome = new List<int>();
	genome.Add(r.Next(0,1000));
    }

    // Used for creating copies.
    public Individual(Individual i){
	r = new Random();
	genome = new List<int>(i.genome);
    }

    public Individual(List<int> i){
	r = new Random();
	genome = new List<int>(i);
    }

    public float getFitness(){
	return this.genome[0] / 1000f;
    }

    public void mutar(){
	float c;
	int s;
	c=(float)(r.NextDouble()*50) % 50; // Over 1000, mean 5% increase/decrease.
	s=r.Next(2);
	if(s==1)
    	    genome[0] += (int)c;
	else
	    genome[0] -= (int)c;
    }

    public String toString(){
	String s = "genome: ";
	foreach(int i in genome){
	    s += i;
	}
	return s;
    }
}
