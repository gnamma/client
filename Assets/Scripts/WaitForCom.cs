using UnityEngine;
using System.Collections;
using System;

public class WaitForCom : CustomYieldInstruction {
    private GNSClient net;
    private string cmd;

    private float waitTime;

    public WaitForCom(ref GNSClient n, string c) {
        net = n;
        cmd = c;

        waitTime = Time.realtimeSinceStartup + 0;
    }

    public override bool keepWaiting {
        get {
            if (Time.realtimeSinceStartup < waitTime) {
                return true;
            }

            return !net.ComAvailable(cmd);
        }
    }
}
