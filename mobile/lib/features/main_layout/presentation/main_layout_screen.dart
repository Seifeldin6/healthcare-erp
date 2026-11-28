import 'package:flutter/material.dart';
import 'package:healthcare_app/core/theme/app_colors.dart';
import 'package:healthcare_app/features/home_screen.dart';
import 'package:healthcare_app/features/main_layout/presentation/widgets/patient_bottom_nav_bar.dart';

class MainLayoutScreen extends StatefulWidget {
  final int initialIndex;

  const MainLayoutScreen({super.key, this.initialIndex = 0});

  @override
  State<MainLayoutScreen> createState() => _MainLayoutScreenState();
}

class _MainLayoutScreenState extends State<MainLayoutScreen> {
  late int _currentIndex;
  late final List<bool> _activatedTabs;

  @override
  void initState() {
    super.initState();
    _currentIndex = widget.initialIndex;
    _activatedTabs = List.generate(4, (index) => index == _currentIndex);
  }

  void _onTabChanged(int index) {
    if (_currentIndex == index) return;
    setState(() {
      _currentIndex = index;
      _activatedTabs[index] = true;
    });
  }

  Widget _buildTab(int index) {
    if (!_activatedTabs[index]) {
      return const SizedBox.shrink();
    }

    switch (index) {
      case 0:
        return const HomeScreen();
      case 1:
        return const _PlaceholderTab(title: 'Pharmacists');
      case 2:
        return const _PlaceholderTab(title: 'Consultations');
      case 3:
        return const _PlaceholderTab(title: 'Profile');
      default:
        return const SizedBox.shrink();
    }
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      backgroundColor: AppColors.scaffoldBackground,
      body: IndexedStack(
        index: _currentIndex,
        children: List.generate(4, _buildTab),
      ),
      bottomNavigationBar: PatientBottomNavBar(
        currentIndex: _currentIndex,
        onDestinationSelected: _onTabChanged,
      ),
    );
  }
}

class _PlaceholderTab extends StatelessWidget {
  final String title;

  const _PlaceholderTab({required this.title});

  @override
  Widget build(BuildContext context) {
    return Center(child: Text(title));
  }
}
