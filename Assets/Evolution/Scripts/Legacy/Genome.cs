using System;
using System.Collections;
using System.Collections.Generic;
using Random = UnityEngine.Random;

namespace SL.Scripts
{
    [Serializable]
    public class Genome
    {
        public float BirthSize;
        public float TimeToBirth;
        public float BirthForce;

        public Genome()
        {
            BirthSize = 100f;
            TimeToBirth = .35f;
            BirthForce = 30f;
        }

        public Genome(float birthSize, float timeToBirth, float birthForce)
        {
            BirthSize = birthSize;
            TimeToBirth = timeToBirth;
            BirthForce = birthForce;
        }


        public Genome Clone()
        {
            Genome genome = new Genome(this.BirthSize, this.TimeToBirth, this.BirthForce);
            return genome;
        }

        public Genome Mutate()
        {
            // if (Random.value > .5f)
            // {
            BirthSize += (Random.value > .5f) ? Random.Range(-.5f, .5f) : 0;
            TimeToBirth += (Random.value > .5f) ? Random.Range(-.02f, .02f) : 0;
            BirthForce += (Random.value > .5f) ? Random.Range(-.15f, .15f) : 0;

            if (BirthSize < 20) BirthSize = 20;
            if (TimeToBirth < .1f) TimeToBirth = .1f;
            if (BirthForce < .1f) BirthForce = .1f;
            // }

            return this;
        }
    }
}