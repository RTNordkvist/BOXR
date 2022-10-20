using BOXR.Data.Models;
using BOXR.DataAccess.Repositories;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BOXR.Domain
{
    public class InbreedingCalculator
    {
        private DogRepository dogRepository;

        private List<DogNode> dogNodesMother = new();
        private List<DogNode> dogNodesFather = new();

        private int numberOfGenerations;

        public double InbreedingCoefficient { get; private set; }

        public InbreedingCalculator(string pedigreeNumber, int numberOfGenerations, DogRepository dogRepository)
        {
            InbreedingCoefficient = 0;
            this.dogRepository = dogRepository;
            this.numberOfGenerations = numberOfGenerations;
            Dog dog = dogRepository.Get(pedigreeNumber);
            CalculateInbreedingCoefficient(dog);
        }
        public InbreedingCalculator(string pedigreeNumber, int numberOfGenerations, DogRepository dogRepository, string mothersPedigreeNumber, string fathersPedigreeNumber)
        {
            InbreedingCoefficient = 0;
            this.dogRepository = dogRepository;
            this.numberOfGenerations = numberOfGenerations;
            Dog dog = new Dog() { PedigreeNumber = pedigreeNumber, MotherPedigreeNumber = mothersPedigreeNumber, FatherPedigreeNumber = fathersPedigreeNumber};
            CalculateInbreedingCoefficient(dog);
        }

        private void CalculateInbreedingCoefficient(Dog dog)
        {
            DogNode baseDog = new DogNode() { PedigreeNumber = dog.PedigreeNumber };

            baseDog.Mother = PopulateAncestorTree(dog.MotherPedigreeNumber, baseDog, 1, dogNodesMother);
            baseDog.Father = PopulateAncestorTree(dog.FatherPedigreeNumber, baseDog, 1, dogNodesFather);

            foreach (var node in dogNodesMother)
            {
                var matchInFathersTree = dogNodesFather.Where(x => x.PedigreeNumber == node.PedigreeNumber).ToList();

                foreach (var match in matchInFathersTree)
                {
                    if (node.Child.PedigreeNumber != match.Child.PedigreeNumber)
                    {
                        InbreedingCoefficient += Math.Pow(0.5, (node.GenerationsFromBase + match.GenerationsFromBase - 1));
                    }
                }
            }
        }

        private DogNode PopulateAncestorTree(string pedigreeNumber, DogNode child, int countGenerations, List<DogNode> listOfNodes)
        {
            Dog rootDog = dogRepository.Get(pedigreeNumber);
            DogNode root = new DogNode() { PedigreeNumber = pedigreeNumber, Child = child, GenerationsFromBase = countGenerations };
            listOfNodes.Add(root);

            if (rootDog != null && countGenerations < numberOfGenerations)
            {
                countGenerations++;

                if (!string.IsNullOrWhiteSpace(rootDog.MotherPedigreeNumber))
                {
                    root.Mother = PopulateAncestorTree(rootDog.MotherPedigreeNumber, root, countGenerations, listOfNodes);
                }
                if (!string.IsNullOrWhiteSpace(rootDog.FatherPedigreeNumber))
                {
                    root.Father = PopulateAncestorTree(rootDog.FatherPedigreeNumber, root, countGenerations, listOfNodes);
                }
            }

            return root;
        }
    }
}
