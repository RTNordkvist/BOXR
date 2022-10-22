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
    public class AncestorTreeLogic
    {
        private DogRepository dogRepository;

        public AncestorTreeLogic(DogRepository dogRepository)
        {
            this.dogRepository = dogRepository;
        }

        /// <summary>
        /// For a fictive dog, the dog parameter should be a Dog-model containg a MotherPedigreeNumber and a FatherPedigreeNumber
        /// </summary>
        public double CalculateInbreedingCoefficient(Dog dog, int numberOfGenerations)
        {
            double inbreedingCoefficient = 0;

            List<DogNode> dogNodesMother = new();
            List<DogNode> dogNodesFather = new();

            DogNode baseDog = new DogNode() { PedigreeNumber = dog.PedigreeNumber };

            // Seperate ancestor trees are created for the mother and father
            baseDog.Mother = PopulateAncestorTree(dog.MotherPedigreeNumber, numberOfGenerations, baseDog, 1, dogNodesMother);
            baseDog.Father = PopulateAncestorTree(dog.FatherPedigreeNumber, numberOfGenerations, baseDog, 1, dogNodesFather);

            // For each node in mother's tree it is checked whether the pedigreeNumber also exists in the father's tree
            foreach (var node in dogNodesMother)
            {
                var matchInFathersTree = dogNodesFather.Where(x => x.PedigreeNumber != null && x.PedigreeNumber == node.PedigreeNumber).ToList();

                // For each pedigreeNumber, that exists in both trees, the contribution for the inbreedingCoefficient is calculated
                foreach (var match in matchInFathersTree)
                {
                    //The inbreedingCoefficient should only be calculated for the shortest path but not the parents, grandparents, etc.
                    if (node.Child.PedigreeNumber != match.Child.PedigreeNumber) 
                    {
                        inbreedingCoefficient += Math.Pow(0.5, (node.GenerationsFromBase + match.GenerationsFromBase - 1));

                        //TODO: Calculate the addtion from inbreeding in any dog appearing in both trees
                    }
                }
            }

            return inbreedingCoefficient;
        }

        public DogNode PopulateAncestorTree(string pedigreeNumber, int numberOfGenerations, DogNode child, int countGenerations, List<DogNode> listOfNodes)
        {
            Dog rootDog = null;
            if (pedigreeNumber != null)
            {
                rootDog = dogRepository.Get(pedigreeNumber);
            }
            
            // A node is added for each link in the ancestor tree whether or not the node is populated with data
            DogNode root = new DogNode() { Name = rootDog?.Name, PedigreeNumber = pedigreeNumber, Child = child, GenerationsFromBase = countGenerations };
            listOfNodes.Add(root);

            // Recursive calls are made until the selected number of generations is reached
            if (countGenerations < numberOfGenerations)
            {
                countGenerations++;

                root.Mother = PopulateAncestorTree(rootDog?.MotherPedigreeNumber, numberOfGenerations, root, countGenerations, listOfNodes);
                root.Father = PopulateAncestorTree(rootDog?.FatherPedigreeNumber, numberOfGenerations, root, countGenerations, listOfNodes);
            }

            return root;
        }
    }
}
