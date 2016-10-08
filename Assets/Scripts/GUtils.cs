﻿using UnityEngine;
using System.Collections;

namespace GUtils {
    public class Point {
        public float x;
        public float y;
        public float z;

        public Point(float x, float y, float z) {
            this.x = x;
            this.y = y;
            this.z = z;
        }
        
        public Point(Vector3 v) {
            x = v.x;
            y = v.y;
            z = v.z;
        }
    }
}