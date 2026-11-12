import 'package:flutter/material.dart';
import 'package:flutter_localizations/flutter_localizations.dart';
import 'package:healthcare_app/generated/l10n.dart';

class HealthcareApp extends StatelessWidget {
  const HealthcareApp({super.key});

  @override
  Widget build(BuildContext context) {
    return MaterialApp(
      title: 'Healthcare App',
      debugShowCheckedModeBanner: false,
      // Localization Configuration (Flutter Intl)
      localizationsDelegates: const [
        S.delegate,
        GlobalMaterialLocalizations.delegate,
        GlobalWidgetsLocalizations.delegate,
        GlobalCupertinoLocalizations.delegate,
      ],
      theme: ThemeData(scaffoldBackgroundColor: Colors.white),
      locale: const Locale('ar'),
      supportedLocales: S.delegate.supportedLocales,
      home: TestScreen(),
    );
  }
}
class TestScreen extends StatelessWidget {
  const TestScreen({super.key});

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
        title: Text(S.of(context).appTitle),
      ),
      body: Center(
        child: Text(S.of(context).appTitle),
      ),
    );
  }
}
