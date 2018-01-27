package com.example.dibby.restaurantapp2;

import android.os.Bundle;
import android.support.v7.app.AppCompatActivity;
import android.text.Editable;
import android.text.TextWatcher;
import android.util.Log;
import android.view.View;
import android.widget.TextView;
import android.widget.Toast;

import com.android.volley.Request;
import com.android.volley.RequestQueue;
import com.android.volley.Response;
import com.android.volley.VolleyError;
import com.android.volley.VolleyLog;
import com.android.volley.toolbox.JsonObjectRequest;
import com.android.volley.toolbox.StringRequest;
import com.android.volley.toolbox.Volley;

import org.json.JSONArray;
import org.json.JSONException;
import org.json.JSONObject;

import java.util.HashMap;
import java.util.Map;

public class CloseOutActivity extends AppCompatActivity {

    String serverURL;
    int tableNumber;
    String orderNumber;
    String tableStatus;
    TextView total;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_close_out);

        // Load food and drink array data
        Bundle bundle = getIntent().getExtras();
        serverURL = bundle.getString("serverURL");

        TextView tableValue = (TextView) findViewById(R.id.tableValue);
        tableValue.addTextChangedListener(new TextWatcher() {
            public void afterTextChanged(Editable s) {}
            public void beforeTextChanged(CharSequence s, int start,
                                          int count, int after) {}

            public void onTextChanged(CharSequence s, int start,
                                      int before, int count) {
                try {
                    tableNumber = Integer.parseInt(s.toString());
                }
                catch (NumberFormatException e){};
                if (tableNumber == 0); //do nothing
                else {
                    Log.d("DEBUG: ", "Selected item is " + s.toString());
                    getTableTotal();
                }
            }
        });

        total = (TextView)findViewById(R.id.totalTextView);
    }

    // Called when user taps Seating button
    public void closeOutButton (View view) {
        closeTable();
    }

    public void getTableTotal(){
        RequestQueue queue = Volley.newRequestQueue(this);
        String url = serverURL + "/tableStatus.php";

        JsonObjectRequest req = new JsonObjectRequest(Request.Method.GET, url,
                null, new Response.Listener<JSONObject>() {

            @Override
            public void onResponse(JSONObject response) {
                Log.d("DEBUG", response.toString());
                String JSONstring;
                JSONstring = response.toString();
                getTableStatus(JSONstring);
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

    public void getTableStatus(String JSONstring) {
        try {
            // Create the root JSONObject from the JSON string.
            JSONObject jsonRootObject = new JSONObject(JSONstring).getJSONObject("object");

            //Get the instance of JSONArray that contains JSONObjects
            JSONArray jsonTableArray = jsonRootObject.optJSONArray("table");

            // Initialize the food and drink arrays based on length
            String tables[][];
            tables = new String[jsonTableArray.length()][3];
            Log.d("DEBUG:", "Table array items: " + jsonTableArray.length());

            //Iterate the jsonArray and populated the tables in the jsonTableArray
            for (int i = 0; i < jsonTableArray.length(); i++) {
                JSONObject jsonObject = jsonTableArray.getJSONObject(i);
                tables[i][0] = jsonObject.optString("tableNumber");
                tables[i][1] = jsonObject.optString("openStatus");
                tables[i][2] = jsonObject.optString("currentOrderNumber");
            }

            tableStatus = tables[tableNumber - 1][1];
            orderNumber = tables[tableNumber - 1][2];

            if (tableStatus.equals("1")) {
                total.setText("Total: $0.00");
                loadOrder();
            }
            else{
                Log.d("DEBUG: ", "Table " + tableNumber + " is closed.");
                Toast.makeText(getApplicationContext(), "This table is not open.", Toast.LENGTH_LONG).show();
                total.setText("Total: $0.00");
            }

        } catch (JSONException e) {
            e.printStackTrace();
        }
    }

    public void loadOrder(){
        Log.d("DEBUG: ", "Loading order for " + orderNumber);
        RequestQueue requestQueue = Volley.newRequestQueue(this);
        String URL = serverURL + "/orderlist.php";
        Log.d("DEBUG: ", "Retrieving order #" + orderNumber + " from " + URL);

        StringRequest stringRequest = new StringRequest(Request.Method.POST, URL,
                new Response.Listener<String>() {
                    @Override
                    public void onResponse(String response) {
                        Log.i("VOLLEY", response);
                        calculateTableTotal(response);
                    }
                },
                new Response.ErrorListener() {
                    @Override
                    public void onErrorResponse(VolleyError error) {
                        Log.e("VOLLEY", error.toString());
                    }
                }) {
            @Override
            protected Map<String, String> getParams() {
                Map<String, String> params = new HashMap<String, String>();
                params.put("orderNumber", orderNumber);
                return params;
            }
        };
        requestQueue.add(stringRequest);
    }

    public void calculateTableTotal(String JSONstring){
        try {
            // Create the root JSONObject from the JSON string.
            JSONObject jsonRootObject = new JSONObject(JSONstring).getJSONObject("object");

            //Get the instance of JSONArray that contains JSONObjects
            JSONArray orderItemArray = jsonRootObject.optJSONArray("order");

            // Initialize the food and drink arrays based on length
            String orderItems[][];
            orderItems = new String[orderItemArray.length()][4];
            Log.d("DEBUG:", "orderItems array items: " + orderItemArray.length());

            //Iterate the jsonArray and populated the order item in the orderItemArray
            for (int i = 0; i < orderItemArray.length(); i++) {
                JSONObject jsonObject = orderItemArray.getJSONObject(i);
                orderItems[i][0] = jsonObject.optString("Item");
                orderItems[i][1] = jsonObject.optString("Quantity");
                orderItems[i][2] = jsonObject.optString("SubmitTime");
                orderItems[i][3] = jsonObject.optString("price");
            }

            double totalCost = 0.00;
            for (int i = 0; i < orderItems.length; i++)
            {
                float x = Float.valueOf(orderItems[i][3]);
                totalCost = totalCost + x;
            }
            String result = String.format("%.2f", totalCost);
            total.setText("Total: $" + result);

        } catch (JSONException e) {
            e.printStackTrace();
        }
    }

    public void closeTable(){
        if(tableNumber > 10 || tableNumber < 1) {
            Toast.makeText(getApplicationContext(), "Not a valid table number.", Toast.LENGTH_SHORT).show();
        }
        else{
            Log.d("DEBUG: ", "Closing table " + tableNumber);
            RequestQueue requestQueue = Volley.newRequestQueue(this);
            String URL = serverURL + "/tableOperation.php";

            StringRequest stringRequest = new StringRequest(Request.Method.POST, URL,
                    new Response.Listener<String>() {
                        @Override
                        public void onResponse(String response) {
                            Log.i("VOLLEY", response);
                            Log.d("DEBUG: ", "table " + tableNumber + " has been closed.");
                            total.setText("Total: $0.00");
                            Toast.makeText(getApplicationContext(), "Table has been closed.", Toast.LENGTH_LONG).show();
                        }
                    },
                    new Response.ErrorListener() {
                        @Override
                        public void onErrorResponse(VolleyError error) {
                            Log.e("VOLLEY", error.toString());
                        }
                    }) {
                @Override
                protected Map<String, String> getParams() {
                    Map<String, String> params = new HashMap<String, String>();

                    params.put("operation", "0");
                    params.put("tableNumber", Integer.toString(tableNumber));

                    return params;
                }
            };
            requestQueue.add(stringRequest);
        }
    }


}
