package com.example.dibby.restaurantapp2;

import android.os.Bundle;
import android.support.v7.app.AppCompatActivity;
import android.util.Log;
import android.widget.TextView;
import android.widget.Toast;

import com.android.volley.Request;
import com.android.volley.RequestQueue;
import com.android.volley.Response;
import com.android.volley.VolleyError;
import com.android.volley.VolleyLog;
import com.android.volley.toolbox.JsonObjectRequest;
import com.android.volley.toolbox.Volley;

import org.json.JSONArray;
import org.json.JSONException;
import org.json.JSONObject;

public class SeatingActivity extends AppCompatActivity {

    String serverURL;
    TextView tablevalue[];
    String tableStatus[][];
    String JSONstring;


    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_seating);

        // Get server url value
        Bundle bundle = getIntent().getExtras();
        serverURL = bundle.getString("serverURL");


        getTableStatus();

    }

    //This is called once the JSON for the table status is received.
    public void onTableResopnse(){
        try {
            // Create the root JSONObject from the JSON string.
            JSONObject jsonRootObject = new JSONObject(JSONstring).getJSONObject("object");

            //Get the instance of JSONArray that contains JSONObjects
            JSONArray jsonTableArray = jsonRootObject.optJSONArray("table");

            // Initialize the food and drink arrays based on length
            tableStatus = new String[jsonTableArray.length()][3];
            Log.d("DEBUG:", "Table array items: " + jsonTableArray.length());

            //Iterate the jsonArray and populated the tables in the jsonTableArray
            for (int i = 0; i < jsonTableArray.length(); i++) {
                JSONObject jsonObject = jsonTableArray.getJSONObject(i);
                tableStatus[i][0] = jsonObject.optString("tableNumber");
                tableStatus[i][1] = jsonObject.optString("openStatus");
                tableStatus[i][2] = jsonObject.optString("currentOrderNumber");
            }

        } catch (JSONException e) {
            e.printStackTrace();
        }
        // Present table values as open or closed.
        TextView table1Status = (TextView)findViewById(R.id.table1Status);
        TextView table2Status = (TextView)findViewById(R.id.table2Status);
        TextView table3Status = (TextView)findViewById(R.id.table3Status);
        TextView table4Status = (TextView)findViewById(R.id.table4Status);
        TextView table5Status = (TextView)findViewById(R.id.table5Status);
        TextView table6Status = (TextView)findViewById(R.id.table6Status);
        TextView table7Status = (TextView)findViewById(R.id.table7Status);
        TextView table8Status = (TextView)findViewById(R.id.table8Status);
        TextView table9Status = (TextView)findViewById(R.id.table9Status);
        TextView table10Status = (TextView)findViewById(R.id.table10Status);

        setTableStatus(table1Status, 0);
        setTableStatus(table2Status, 1);
        setTableStatus(table3Status, 2);
        setTableStatus(table4Status, 3);
        setTableStatus(table5Status, 4);
        setTableStatus(table6Status, 5);
        setTableStatus(table7Status, 6);
        setTableStatus(table8Status, 7);
        setTableStatus(table9Status, 8);
        setTableStatus(table10Status, 9);


    }

    // This is the call to server to get the JSON for drinks
    public void getTableStatus(){
        RequestQueue queue = Volley.newRequestQueue(this);
        String url = serverURL + "/tableStatus.php";

        JsonObjectRequest req = new JsonObjectRequest(Request.Method.GET,url,
                null, new Response.Listener<JSONObject>() {

            @Override
            public void onResponse(JSONObject response) {
                Log.d("DEBUG", response.toString());
                JSONstring = response.toString();
                onTableResopnse();
                Toast.makeText(getApplicationContext(), "Tables loaded.", Toast.LENGTH_SHORT).show();
            }
        }, new Response.ErrorListener() {
            @Override
            public void onErrorResponse(VolleyError error) {
                VolleyLog.d("DEBUG", "Error: " + error.getMessage());
                Log.e("EXCEPTPTION", "Site Info Error: " + error.getMessage());
                Toast.makeText(getApplicationContext(), "Data load failure - check URL", Toast.LENGTH_LONG).show();
            }
        });

        queue.add(req);
    }

    public void setTableStatus (TextView textView, int tableNumber){
        if (tableStatus[tableNumber][1].equals("0")){
            textView.setText("Closed");
            //textView.setTextColor(Color.BLUE);
        }
        else
            textView.setText("Open");
    }
}
